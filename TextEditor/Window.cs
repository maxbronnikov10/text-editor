using System;
using System.Drawing;
using System.Windows.Forms;

namespace TextEditor
{
    class Window : Form
    {
        private Model model;
        private ReadonlyListWithIndex<int> list;
        private string path;
        private RichTextBox mainFrame;
        private ToolStripMenuItem saveAsMenuItem;
        private TextBox searchTextBox;

        public Window()
        {
            model = new Model();

            MenuStrip menu = new MenuStrip();
            menu.Location = new Point(0,0);

            ToolStripMenuItem fileMenuItem = new ToolStripMenuItem("File");
            ToolStripMenuItem openMenuItem = new ToolStripMenuItem("Open");
            ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("Save");
            saveAsMenuItem = new ToolStripMenuItem("Save as");
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit");

            fileMenuItem.DropDownItems.Add(openMenuItem);
            fileMenuItem.DropDownItems.Add(saveMenuItem);
            fileMenuItem.DropDownItems.Add(saveAsMenuItem);
            fileMenuItem.DropDownItems.Add(new ToolStripSeparator());
            fileMenuItem.DropDownItems.Add(exitMenuItem);
            menu.Items.Add(fileMenuItem);

            ToolStripMenuItem searchMenuItem = new ToolStripMenuItem("Search");
            menu.Items.Add(searchMenuItem);

            Panel functionsTable = new Panel();
            functionsTable.Location = new Point(0, 35);
            functionsTable.Size = new Size(ClientSize.Width, 35);
           
            Button saveButton = new Button() { Image =  Image.FromFile("..//..//img//save.png") };
            saveButton.FlatStyle = FlatStyle.Flat;
            saveButton.Location = new Point(5, 0);
            saveButton.Size = new Size(35, 35);

            Button openButton = new Button() { Image = Image.FromFile("..//..//img//open.png") };
            openButton.FlatStyle = FlatStyle.Flat;
            openButton.Location = new Point(45, 0);
            openButton.Size = new Size(35, 35);

            searchTextBox = new TextBox();
            searchTextBox.Location = new Point(85, 10);

            Button searchButton = new Button() { Image = Image.FromFile("..//..//img//search.png") };
            searchButton.FlatStyle = FlatStyle.Flat;
            searchButton.Size = new Size(35, 35);
            
            Button previousButton = new Button() { Image = Image.FromFile("..//..//img//left_arrow.png") };
            previousButton.FlatStyle = FlatStyle.Flat;
            previousButton.Size = new System.Drawing.Size(35, 35);

            Button nextButton = new Button() { Image = Image.FromFile("..//..//img//right_arrow.png") };
            nextButton.FlatStyle = FlatStyle.Flat;
            nextButton.Size = new System.Drawing.Size(35, 35);

            CheckBox isUseRegexcheckBox = new CheckBox() {  Text = "Use regex" };

            functionsTable.Controls.Add(saveButton);
            functionsTable.Controls.Add(openButton);
            functionsTable.Controls.Add(searchTextBox);
            functionsTable.Controls.Add(searchButton);
            functionsTable.Controls.Add(previousButton);
            functionsTable.Controls.Add(nextButton);
            functionsTable.Controls.Add(isUseRegexcheckBox);

            mainFrame = new RichTextBox();
            mainFrame.Location = new Point(0,75);


            Controls.Add(menu);
            Controls.Add(functionsTable);
            Controls.Add(mainFrame);

            MinimumSize = new Size(600, 300);
            Text = "Text Editor";
            Resize += (sender,args) => {
                menu.Size = new Size(ClientSize.Width, 35);
                mainFrame.Size = new Size(ClientSize.Width, ClientSize.Height - 75);
                functionsTable.Size = new Size(ClientSize.Width, 40);
                searchTextBox.Size = new Size(ClientSize.Width / 2, 35);
                searchButton.Location = new Point(90 + searchTextBox.Size.Width, 0);
                previousButton.Location = new Point(40 + searchButton.Location.X, 0);
                nextButton.Location = new Point(40 + previousButton.Location.X, 0);
                isUseRegexcheckBox.Location = new Point(40 + nextButton.Location.X, 10);
            };
            Load += (sender, args) => OnResize(new EventArgs());
            openMenuItem.Click += OpenFile_Click;
            saveMenuItem.Click += SaveFile_Click;
            saveAsMenuItem.Click += SaveFile_Click;
            exitMenuItem.Click += (sender, args) => model.Exit();
            searchMenuItem.Click += Search_Click;
            saveButton.Click += SaveFile_Click;
            openButton.Click += OpenFile_Click;
            searchButton.Click += Search_Click;
            previousButton.Click += (sender, args) =>
            {
                if (list == null) return;
                list.MovePrevious();
                mainFrame.SelectionStart = list.GetItem();
            };
            nextButton.Click += (sender, args) =>
            {
                if (list == null) return;
                list.MoveNext();
                mainFrame.SelectionStart = list.GetItem();
                mainFrame.SelectionLength = searchTextBox.Text.Length;
                mainFrame.SelectionColor = Color.Red;
            };

        }

        private void Search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchTextBox.Text) && !string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                list = new ReadonlyListWithIndex<int>(0, model.FindAll(mainFrame.Text, searchTextBox.Text));
                mainFrame.SelectionStart = list.GetItem();
                mainFrame.SelectionLength = searchTextBox.Text.Length;
                mainFrame.SelectionColor = Color.Red;
            }
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            if(path == null || (sender as ToolStripMenuItem) == saveAsMenuItem)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "TXT file (*.txt)|*.txt";
                saveFileDialog.ShowDialog();
                path = saveFileDialog.FileName;
            }
            model.SaveFile(path, mainFrame.Text);
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TXT file (*.txt)|*.txt";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName == "") return;
            path = openFileDialog.FileName;
            mainFrame.Text = model.OpenFile(path);
        }

        [STAThread]
        static void Main()
        {
            Application.Run(new Window());
        }
    }
}
