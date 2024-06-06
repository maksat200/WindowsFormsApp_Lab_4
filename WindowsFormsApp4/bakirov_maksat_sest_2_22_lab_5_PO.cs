using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private string selectedFolderPath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string imagePath = @"C:\Users\bakirov_maksat\OneDrive\Desktop\R.jpeg"; 

            this.BackgroundImage = Image.FromFile(imagePath);
            this.BackgroundImageLayout = ImageLayout.Stretch; 
            
            InitializeFormComponents();
        }

        private void InitializeFormComponents()
        {
            this.Text = "File Manager";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size((int)(Screen.PrimaryScreen.WorkingArea.Width * 0.8), (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.8));

            ToolStripMenuItem aboutMenuItem = new ToolStripMenuItem("About");
            aboutMenuItem.Font = new System.Drawing.Font("Arial" ,10 ); 
            aboutMenuItem.Click += AboutMenuItem_Click;

           
            MenuStrip menuStrip = new MenuStrip();
           
            menuStrip.Items.Add(aboutMenuItem);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            Button selectFolderButton = new Button();
            selectFolderButton.Text = "Select Folder";
            selectFolderButton.Location = new Point(30, 30);
            selectFolderButton.BackColor = Color.FromArgb(111, 10, 130);
            selectFolderButton.Click += SelectFolderButton_Click;
            this.Controls.Add(selectFolderButton);

            TextBox folderPathTextBox = new TextBox();
            folderPathTextBox.Name = "folderPathTextBox";
            folderPathTextBox.ReadOnly = true;
            folderPathTextBox.Location = new Point(150, 20);
            folderPathTextBox.BackColor = Color.FromArgb(111, 10, 130);
            folderPathTextBox.Size = new Size(400, 20);
            this.Controls.Add(folderPathTextBox);

            ListBox subfoldersListBox = new ListBox();
            subfoldersListBox.BackColor = Color.FromArgb(111, 10, 130);
            subfoldersListBox.Name = "subfoldersListBox";
            subfoldersListBox.Location = new Point(20, 60);
            subfoldersListBox.Size = new Size(200, 300);
            subfoldersListBox.DoubleClick += SubfoldersListBox_DoubleClick;
            this.Controls.Add(subfoldersListBox);

            DataGridView fileDataGridView = new DataGridView();

            fileDataGridView.BackColor = Color.FromArgb(100, 10, 130);
            fileDataGridView.Font = new System.Drawing.Font("Arial", 10);
            fileDataGridView.Name = "fileDataGridView";
            fileDataGridView.Location = new Point(250, 60);
            fileDataGridView.Size = new Size(600, 300);
            fileDataGridView.AutoGenerateColumns = false;

            fileDataGridView.Columns.Add("Name", "Name");
            fileDataGridView.Columns.Add("LastModified", "Last Modified");
            fileDataGridView.Columns.Add("Size", "Size (bytes)");
            fileDataGridView.Columns.Add("Processing", "Processing");

            fileDataGridView.ReadOnly = true;
            fileDataGridView.CellDoubleClick += FileDataGridView_CellDoubleClick;
            this.Controls.Add(fileDataGridView);

            Button processFilesButton = new Button();
            processFilesButton.Name = "processFilesButton";
            processFilesButton.Text = "Process Files";
            processFilesButton.Location = new Point(20, 380);
            processFilesButton.BackColor = Color.FromArgb(111, 10, 130);
            processFilesButton.Click += ProcessFilesButton_Click;
            processFilesButton.Visible = false;
            this.Controls.Add(processFilesButton);
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Name: Bakirov Maksat\nEmail: bakirov_m@auca.kg\nPhone:+996 709237656\nAdress: Mederova 161a.", "About");
        }

        private void SelectFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                selectedFolderPath = folderBrowserDialog.SelectedPath;

                TextBox folderPathTextBox = this.Controls.Find("folderPathTextBox", true).FirstOrDefault() as TextBox;
                if (folderPathTextBox != null)
                {
                    folderPathTextBox.Text = selectedFolderPath;
                }

                ListBox subfoldersListBox = this.Controls.Find("subfoldersListBox", true).FirstOrDefault() as ListBox;
                if (subfoldersListBox != null)
                {
                    subfoldersListBox.Items.Clear();
                    string[] subfolders = Directory.GetDirectories(selectedFolderPath);
                    foreach (string subfolder in subfolders)
                    {
                        subfoldersListBox.Items.Add(Path.GetFileName(subfolder));
                    }
                }

                DataGridView fileDataGridView = this.Controls.Find("fileDataGridView", true).FirstOrDefault() as DataGridView;
                if (fileDataGridView != null)
                {
                    PopulateFilesDataGridView(selectedFolderPath, fileDataGridView);
                }

                Button processFilesButton = this.Controls.Find("processFilesButton", true).FirstOrDefault() as Button;
                if (processFilesButton != null)
                {
                    processFilesButton.Visible = true;
                }
            }
        }

        private void SubfoldersListBox_DoubleClick(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null && listBox.SelectedItem != null)
            {
                string selectedFolder = Path.Combine(selectedFolderPath, listBox.SelectedItem.ToString());
                if (Directory.Exists(selectedFolder))
                {
                    ShowFolderDetailsForm(selectedFolder);
                }
            }
        }

        private void ShowFolderDetailsForm(string folderPath)
        {
            
            Form folderDetailsForm = new Form();
            folderDetailsForm.Text = "Folder Details";
            folderDetailsForm.Size = new Size(400, 200);
            folderDetailsForm.StartPosition = FormStartPosition.CenterParent;

            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

          
            Label nameLabel = new Label();
            nameLabel.Text = "Name:";
            nameLabel.Location = new Point(20, 20);
            folderDetailsForm.Controls.Add(nameLabel);

            TextBox nameTextBox = new TextBox();
            nameTextBox.Text = directoryInfo.Name;
            nameTextBox.Location = new Point(150, 20);
            nameTextBox.ReadOnly = true;
            nameTextBox.Size = new Size(200, 20);
            folderDetailsForm.Controls.Add(nameTextBox);

            Label lastModifiedLabel = new Label();
            lastModifiedLabel.Text = "Last Modified:";
            lastModifiedLabel.Location = new Point(20, 50);
            folderDetailsForm.Controls.Add(lastModifiedLabel);

            TextBox lastModifiedTextBox = new TextBox();
            lastModifiedTextBox.Text = directoryInfo.LastWriteTime.ToString();
            lastModifiedTextBox.Location = new Point(150, 50);
            lastModifiedTextBox.ReadOnly = true;
            lastModifiedTextBox.Size = new Size(200, 20);
            folderDetailsForm.Controls.Add(lastModifiedTextBox);

  
            folderDetailsForm.ShowDialog();
        }

        private void FileDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView dataGridView = sender as DataGridView;
                if (dataGridView != null && dataGridView.Rows[e.RowIndex].Cells["Name"].Value != null)
                {
                    string fileName = dataGridView.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                    DialogResult result = MessageBox.Show($"Do you want to duplicate file '{fileName}' in the same folder?", "Duplicate File", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        string filePath = Path.Combine(selectedFolderPath, fileName);
                        string newFilePath = Path.Combine(selectedFolderPath, $"Copy_of_{fileName}");
                        File.Copy(filePath, newFilePath);
                        MessageBox.Show($"File '{fileName}' duplicated as 'Copy_of_{fileName}'.", "File Duplicated");
                        
                        PopulateFilesDataGridView(selectedFolderPath, dataGridView);
                    }
                }
            }
        }

        private async void ProcessFilesButton_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = this.Controls.Find("fileDataGridView", true).FirstOrDefault() as DataGridView;
            if (dataGridView != null && dataGridView.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
           
                    Random random = new Random();
                    int processingTime = random.Next(1, dataGridView.Rows.Count + 1);

                
                    row.Cells["Processing"].Value = processingTime;

                 
                    await Task.Delay(processingTime * 1000);

                    
                    row.Cells["Processing"].Value = processingTime;
                }
            }
        }

        private void PopulateFilesDataGridView(string folderPath, DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                dataGridView.Rows.Add(fileInfo.Name, fileInfo.LastWriteTime, fileInfo.Length, "");
            }
        }
    }
}
