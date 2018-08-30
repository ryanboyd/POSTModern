using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using java.util;
using edu.stanford.nlp.tagger.maxent;
using edu.stanford.nlp.ling;


namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {


        public static string jarRoot = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), @"stanford-postagger-full-2018-02-27");
        public static string modelsDirectory = Path.Combine(jarRoot, @"models");


        //initialize the space for our dictionary data
        BackgroundWorkerData BGData = new BackgroundWorkerData();



        //this is what runs at initialization
        public Form1()
        {

            InitializeComponent();

            foreach (var encoding in Encoding.GetEncodings())
            {
                EncodingDropdown.Items.Add(encoding.Name);
            }

            try
            {
                EncodingDropdown.SelectedIndex = EncodingDropdown.FindStringExact("utf-8");
            }
            catch
            {
                EncodingDropdown.SelectedIndex = EncodingDropdown.FindStringExact(Encoding.Default.BodyName);
            }


            //automatically read in all of the models
            DirectoryInfo d = new DirectoryInfo(modelsDirectory);

            foreach (var file in d.GetFiles("*.*"))
            {
                if (file.Name.EndsWith(".tagger") || file.Name.EndsWith(".model")) ModelSelectionBox.Items.Add(file.Name);
            }

            if (ModelSelectionBox.Items.Count == 0)
            {
                MessageBox.Show("You do not appear to have any model files with the software. Please ensure that you have the full StanfordNLP package and its associated models included with this software.", "No models found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            else
            {

                try
                {
                    ModelSelectionBox.SelectedIndex = ModelSelectionBox.FindStringExact("english-bidirectional-distsim.tagger");
                }
                catch
                {
                    ModelSelectionBox.SelectedIndex = 0;
                }

                
            }



        }







        private void StartButton_Click(object sender, EventArgs e)
        {


            if (!uint.TryParse(SegmentTextbox.Text, out uint n) || int.Parse(SegmentTextbox.Text) < 1)
            {
                MessageBox.Show("Your selected number of segments must be a postive integer (i.e., a whole number greater than zero).", "Segmentation Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FolderBrowser.Description = "Please choose the location of your .txt files to analyze";
            if (FolderBrowser.ShowDialog() != DialogResult.Cancel) {

                BGData.TextFileFolder = FolderBrowser.SelectedPath.ToString();

                if (BGData.TextFileFolder != "")
                {

                    saveFileDialog.FileName = "POSTModern_Results.csv";

                    saveFileDialog.InitialDirectory = BGData.TextFileFolder;
                    if (saveFileDialog.ShowDialog() != DialogResult.Cancel) {


                        BGData.OutputFileLocation = saveFileDialog.FileName;
                        BGData.SelectedModel = ModelSelectionBox.SelectedItem.ToString();
                        BGData.OutputTaggedText = SavePOStextCheckbox.Checked;
                        BGData.OrderedPOSTagText = IncludeOrderedPOSTagsCheckbox.Checked;
                        BGData.NormalizeOutput = NormalizeOutputCheckbox.Checked;
                        BGData.NumSegments = int.Parse(SegmentTextbox.Text);

                        if (BGData.OutputFileLocation != "") {


                            StartButton.Enabled = false;
                            ScanSubfolderCheckbox.Enabled = false;
                            EncodingDropdown.Enabled = false;
                            ModelSelectionBox.Enabled = false;
                            SavePOStextCheckbox.Enabled = false;
                            IncludeOrderedPOSTagsCheckbox.Enabled = false;
                            NormalizeOutputCheckbox.Enabled = false;
                            SegmentTextbox.Enabled = false;

                            BgWorker.RunWorkerAsync(BGData);
                        }
                    }
                }

            }



        }






        private void BgWorkerClean_DoWork(object sender, DoWorkEventArgs e)
        {


            BackgroundWorkerData BGData = (BackgroundWorkerData)e.Argument;


            //report what we're working on
            FilenameLabel.Invoke((MethodInvoker)delegate
            {
                FilenameLabel.Text = "Loading model...";
            });


            //set up our sentence boundary detection
            Regex SentenceSplitter = new Regex(@"(?<!\w\.\w.)(?<![A-Z][a-z]\.)(?<=\.|\?|\!)\s", RegexOptions.Compiled);

            //selects the text encoding based on user selection
            Encoding SelectedEncoding = null;
            this.Invoke((MethodInvoker)delegate ()
            {
                SelectedEncoding = Encoding.GetEncoding(EncodingDropdown.SelectedItem.ToString());
            });



            //get the list of files
            var SearchDepth = SearchOption.TopDirectoryOnly;
            if (ScanSubfolderCheckbox.Checked)
            {
                SearchDepth = SearchOption.AllDirectories;
            }
            var files = Directory.EnumerateFiles(BGData.TextFileFolder, "*.txt", SearchDepth);



            try {

                var tagger = new MaxentTagger(modelsDirectory + @"/" + BGData.SelectedModel);

                int NumberOfTagsInModel = tagger.numTags();

                List<string> tags_list_header = new List<string>();
                List<string> tags_list = new List<string>();


                for (int i = 0; i < NumberOfTagsInModel; i++)
                {
                    tags_list_header.Add("\"" + tagger.getTag(i) + "\"");
                    tags_list.Add(tagger.getTag(i));
                }

                tags_list_header.Sort();
                tags_list.Sort();

                string[] tags_array = tags_list.ToArray();

                


                //open up the output file
                using (StreamWriter outputFile = new StreamWriter(new FileStream(BGData.OutputFileLocation, FileMode.Create), SelectedEncoding))
                {

                    

                        //write the header row to the output file
                        StringBuilder HeaderString = new StringBuilder();
                        HeaderString.Append("\"Filename\",\"Segment\",\"TokenCount\",\"SentenceCount\"," + string.Join(",", tags_list_header.ToArray()));

                        if (BGData.OutputTaggedText) HeaderString.Append(",\"TaggedText\"");
                        if (BGData.OrderedPOSTagText) HeaderString.Append(",\"OrderedPOSTags\"");

                        outputFile.WriteLine(HeaderString.ToString());
                

                        foreach (string fileName in files)
                        {

                            //set up our variables to report
                            string Filename_Clean = Path.GetFileName(fileName);
                            

                            //report what we're working on
                            FilenameLabel.Invoke((MethodInvoker)delegate
                            {
                                FilenameLabel.Text = "Analyzing: " + Filename_Clean;
                            });




                            //read in the text file, convert everything to lowercase
                            var InputText = System.IO.File.ReadAllText(fileName, SelectedEncoding).Trim();

                            var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(InputText)).toArray();


                            
                            //now that we know how many sentences we have, we can figure out the segmentation
                            double SentencesPerSegment = 1.0;
                            int NumberOfSegments = BGData.NumSegments;
                            if (NumberOfSegments > sentences.Length) NumberOfSegments = sentences.Length;

                            if (sentences.Length > 0) SentencesPerSegment = sentences.Length / (double)NumberOfSegments;


                             List<List<ArrayList>> Sentences_Segmented = new List<List<ArrayList>>();

                            int SegmentCounter = 1;
                            //int SentenceNumberTracker = 0;
                            for (int i = 0; i < sentences.Length; i++)
                            {

                                if (Sentences_Segmented.Count < SegmentCounter) Sentences_Segmented.Add(new List<ArrayList>());

                                Sentences_Segmented[SegmentCounter - 1].Add((ArrayList)sentences[i]);
                                //SentenceNumberTracker++;

                                if (i + 1 >= SegmentCounter * SentencesPerSegment)
                                {
                                    SegmentCounter++;
                                    //SentenceNumberTracker = 0;
                                }
                                
                            }


                            sentences = null;





                            //     _                _                 _____         _   
                            //    / \   _ __   __ _| |_   _ _______  |_   _|____  _| |_ 
                            //   / _ \ | '_ \ / _` | | | | |_  / _ \   | |/ _ \ \/ / __|
                            //  / ___ \| | | | (_| | | |_| |/ /  __/   | |  __/>  <| |_ 
                            // /_/   \_\_| |_|\__,_|_|\__, /___\___|   |_|\___/_/\_\\__|
                            //                        |___/                             




                            for (int i = 0; i < NumberOfSegments; i++) {


                                Dictionary<string, int> POSSums = new Dictionary<string, int>();
                                for (int j = 0; j < NumberOfTagsInModel; j++) POSSums.Add(tags_array[j], 0);


                                StringBuilder TaggedText = new StringBuilder();
                                StringBuilder OrderedPOSTags = new StringBuilder();

                                int TotalSentences = Sentences_Segmented[i].Count;
                                int TotalWC = 0;


                                foreach (ArrayList sentence in Sentences_Segmented[i])
                                {


                                    
                                    var taggedSentence = tagger.tagSentence(sentence);


                                    Iterator it = taggedSentence.iterator();                           

                                  
 
                                

                                    while (it.hasNext())
                                    {


                                        TaggedWord token = (TaggedWord)it.next();

                                        if (BGData.OutputTaggedText) TaggedText.Append(token.toString() + " ");
                                        if (BGData.OrderedPOSTagText) OrderedPOSTags.Append(token.tag() + " ");


                                        POSSums[token.tag()] += 1;
                                        TotalWC += 1;
                                    
                                        //MessageBox.Show(token.word());  
                                    

                                    }

                                    TaggedText.Append(Environment.NewLine);
                                    OrderedPOSTags.Append(Environment.NewLine);

                                }







                                // __        __    _ _          ___        _               _   
                                // \ \      / / __(_) |_ ___   / _ \ _   _| |_ _ __  _   _| |_ 
                                //  \ \ /\ / / '__| | __/ _ \ | | | | | | | __| '_ \| | | | __|
                                //   \ V  V /| |  | | ||  __/ | |_| | |_| | |_| |_) | |_| | |_ 
                                //    \_/\_/ |_|  |_|\__\___|  \___/ \__,_|\__| .__/ \__,_|\__|
                                //                                            |_|              




                                string[] OutputString = new string[4];
                                OutputString[0] = "\"" + Filename_Clean + "\"";
                                OutputString[1] = (i + 1).ToString();
                                OutputString[2] = TotalWC.ToString();
                                OutputString[3] = TotalSentences.ToString();

                                int include_tagged_text = 0;
                                int include_ordered_pos = 0;
                                if (BGData.OutputTaggedText) include_tagged_text = 1;
                                if (BGData.OrderedPOSTagText) include_ordered_pos = 1;

                                string[] TagOutputString = new string[NumberOfTagsInModel + include_tagged_text + include_ordered_pos];

                                for(int j = 0; j < NumberOfTagsInModel; j++)
                                {
                                    if (BGData.NormalizeOutput && TotalWC > 0)
                                    {
                                        TagOutputString[j] = RoundUp(POSSums[tags_array[j]] * 100 / (double)TotalWC, 5).ToString();
                                    }
                                    else
                                    {
                                        TagOutputString[j] = POSSums[tags_array[j]].ToString();
                                    }
                                        
                                }

                                if (BGData.OutputTaggedText) TagOutputString[TagOutputString.Length - include_tagged_text - include_ordered_pos] = "\"" +TaggedText.ToString().Replace("\"", "\"\"") + "\"";
                                if (BGData.OrderedPOSTagText) TagOutputString[TagOutputString.Length - include_ordered_pos] = "\"" + OrderedPOSTags.ToString().Replace("\"", "\"\"") + "\"";

                                outputFile.WriteLine(String.Join(",", MergeOutputArrays(OutputString, TagOutputString)));



                            }



                        //end of the "for each file" loop
                        }


                    

                }

            }
            catch (OutOfMemoryException OOM)
            {
                MessageBox.Show("One or more of your files caused an Out of Memory error. This means that you do not have enough RAM to process the current file. This is often caused by extremely complex / messy language samples with run-on sentences or other peculiar constructions, paired with a computer that does not have enough RAM to handle such processing.", "Out of Memory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                MessageBox.Show("POSTModern encountered an issue somewhere while trying to analyze your texts. The most common cause of this is trying to open your output file while POSTModern is still running. Did any of your input files move, or is your output file being opened/modified by another application?", "Error while analyzing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }


        //when the bgworker is done running, we want to re-enable user controls and let them know that it's finished
        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StartButton.Enabled = true;
            ScanSubfolderCheckbox.Enabled = true;
            EncodingDropdown.Enabled = true;
            ModelSelectionBox.Enabled = true;
            SavePOStextCheckbox.Enabled = true;
            IncludeOrderedPOSTagsCheckbox.Enabled = true;
            NormalizeOutputCheckbox.Enabled = true;
            SegmentTextbox.Enabled = true;
            FilenameLabel.Text = "Finished!";
            MessageBox.Show("POSTModern has finished analyzing your texts.", "Analysis Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }







        public class BackgroundWorkerData
        {

            public string TextFileFolder { get; set; }
            public string OutputFileLocation { get; set; }
            public string SelectedModel { get; set; }
            public bool OutputTaggedText { get; set; }
            public bool OrderedPOSTagText { get; set; }
            public bool NormalizeOutput { get; set; }
            public int NumSegments { get; set; }

        }

        //https://stackoverflow.com/a/24367618
        string AddSuffix(string filename, string suffix)
        {
            string fDir = Path.GetDirectoryName(filename);
            string fName = Path.GetFileNameWithoutExtension(filename);
            string fExt = Path.GetExtension(filename);
            return Path.Combine(fDir, String.Concat(fName, suffix, fExt));
        }

        string[] MergeOutputArrays(string[] Arr1, string[] Arr2)
        {
            string[] Arr3 = new string[Arr1.Length + Arr2.Length];
            Arr1.CopyTo(Arr3, 0);
            Arr2.CopyTo(Arr3, Arr1.Length);

            return (Arr3);
        }

        //https://stackoverflow.com/a/7075225
        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }












    }



}
