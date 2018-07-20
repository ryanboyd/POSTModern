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
        DictionaryData DictData = new DictionaryData();



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




            FolderBrowser.Description = "Please choose the location of your .txt files to analyze";
            if (FolderBrowser.ShowDialog() != DialogResult.Cancel) {

                DictData.TextFileFolder = FolderBrowser.SelectedPath.ToString();

                if (DictData.TextFileFolder != "")
                {

                    saveFileDialog.FileName = "POSTModern_Results.csv";

                    saveFileDialog.InitialDirectory = DictData.TextFileFolder;
                    if (saveFileDialog.ShowDialog() != DialogResult.Cancel) {


                        DictData.OutputFileLocation = saveFileDialog.FileName;
                        DictData.SelectedModel = ModelSelectionBox.SelectedItem.ToString();
                        DictData.OutputTaggedText = SavePOStextCheckbox.Checked;
                        DictData.NormalizeOutput = NormalizeOutputCheckbox.Checked;

                        if (DictData.OutputFileLocation != "") {


                            StartButton.Enabled = false;
                            ScanSubfolderCheckbox.Enabled = false;
                            EncodingDropdown.Enabled = false;
                            ModelSelectionBox.Enabled = false;
                            SavePOStextCheckbox.Enabled = false;
                            NormalizeOutputCheckbox.Enabled = false;

                            BgWorker.RunWorkerAsync(DictData);
                        }
                    }
                }

            }



        }






        private void BgWorkerClean_DoWork(object sender, DoWorkEventArgs e)
        {


            DictionaryData DictData = (DictionaryData)e.Argument;


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
            var files = Directory.EnumerateFiles(DictData.TextFileFolder, "*.txt", SearchDepth);



            try {

                var tagger = new MaxentTagger(modelsDirectory + @"/" + DictData.SelectedModel);

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
                using (StreamWriter outputFile = new StreamWriter(new FileStream(DictData.OutputFileLocation, FileMode.Create), SelectedEncoding))
                {

                    

                        //write the header row to the output file
                        StringBuilder HeaderString = new StringBuilder();
                        HeaderString.Append("\"Filename\",\"TokenCount\",\"SentenceCount\"," + string.Join(",", tags_list_header.ToArray()));

                        if (DictData.OutputTaggedText) HeaderString.Append(",\"TaggedText\"");

                        outputFile.WriteLine(HeaderString.ToString());
                

                        foreach (string fileName in files)
                        {

                            //set up our variables to report
                            string Filename_Clean = Path.GetFileName(fileName);
                            Dictionary<string, int> POSSums = new Dictionary<string, int>();

                            for (int i = 0; i < NumberOfTagsInModel; i++) POSSums.Add(tags_array[i], 0);

                            //report what we're working on
                            FilenameLabel.Invoke((MethodInvoker)delegate
                            {
                                FilenameLabel.Text = "Analyzing: " + Filename_Clean;
                            });




                            //read in the text file, convert everything to lowercase
                            var InputText = System.IO.File.ReadAllText(fileName, SelectedEncoding).Trim();

                            var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(InputText)).toArray();

                            int TotalSentences = sentences.Length;
                            int TotalWC = 0;

                            StringBuilder TaggedText = new StringBuilder();


                            //     _                _                 _____         _   
                            //    / \   _ __   __ _| |_   _ _______  |_   _|____  _| |_ 
                            //   / _ \ | '_ \ / _` | | | | |_  / _ \   | |/ _ \ \/ / __|
                            //  / ___ \| | | | (_| | | |_| |/ /  __/   | |  __/>  <| |_ 
                            // /_/   \_\_| |_|\__,_|_|\__, /___\___|   |_|\___/_/\_\\__|
                            //                        |___/                             

                            foreach (ArrayList sentence in sentences)
                                {
                                var taggedSentence = tagger.tagSentence(sentence);

                                if (DictData.OutputTaggedText) TaggedText.Append(taggedSentence.ToString() + " ");

                                Iterator it = taggedSentence.iterator();

                                while (it.hasNext())
                                {


                                    TaggedWord token = (TaggedWord)it.next();

                                    POSSums[token.tag()] += 1;
                                    TotalWC += 1;
                                    
                                    //MessageBox.Show(token.word());  
                                    

                                }

                            }







                            // __        __    _ _          ___        _               _   
                            // \ \      / / __(_) |_ ___   / _ \ _   _| |_ _ __  _   _| |_ 
                            //  \ \ /\ / / '__| | __/ _ \ | | | | | | | __| '_ \| | | | __|
                            //   \ V  V /| |  | | ||  __/ | |_| | |_| | |_| |_) | |_| | |_ 
                            //    \_/\_/ |_|  |_|\__\___|  \___/ \__,_|\__| .__/ \__,_|\__|
                            //                                            |_|              




                            string[] OutputString = new string[3];
                            OutputString[0] = "\"" + Filename_Clean + "\"";
                            OutputString[1] = TotalWC.ToString();
                            OutputString[2] = TotalSentences.ToString();

                            int include_tagged_text = 0;
                            if (DictData.OutputTaggedText) include_tagged_text = 1;

                            string[] TagOutputString = new string[NumberOfTagsInModel + include_tagged_text];

                            for(int i = 0; i < NumberOfTagsInModel; i++)
                            {
                                if (DictData.NormalizeOutput && TotalWC > 0)
                                {
                                    TagOutputString[i] = RoundUp(POSSums[tags_array[i]] * 100 / (double)TotalWC, 5).ToString();
                                }
                                else
                                {
                                    TagOutputString[i] = POSSums[tags_array[i]].ToString();
                                }
                                        
                            }

                            if (DictData.OutputTaggedText) TagOutputString[TagOutputString.Length - 1] = "\"" +TaggedText.ToString().Replace("\"", "\"\"") + "\"";


                            outputFile.WriteLine(String.Join(",", MergeOutputArrays(OutputString, TagOutputString)));








                        }


                    

                }

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
            NormalizeOutputCheckbox.Enabled = true;
            FilenameLabel.Text = "Finished!";
            MessageBox.Show("POSTModern has finished analyzing your texts.", "Analysis Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }







        public class DictionaryData
        {

            public string TextFileFolder { get; set; }
            public string OutputFileLocation { get; set; }
            public string SelectedModel { get; set; }
            public bool OutputTaggedText { get; set; }
            public bool NormalizeOutput { get; set; }

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
