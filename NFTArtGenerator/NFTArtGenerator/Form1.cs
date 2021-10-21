using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;


namespace NFTArtGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ((ListBox)clbLayers).DisplayMember = "DisplayText";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddLayer_Click(object sender, EventArgs e)
        {
            var displayText = $"Order # {Convert.ToInt32(txtOrder.Text)}, Location: {txtInputFolder.Text}";
            var item = new LayerInformation(Convert.ToInt32(txtOrder.Text), txtInputFolder.Text, true, displayText);
            clbLayers.Items.Add(item);
            txtOrder.Text = string.Empty;
            txtInputFolder.Text = string.Empty;
        }

        private void btnOpenInputFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1 = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtInputFolder.Text = folderBrowserDialog1.SelectedPath;
                //Environment.SpecialFolder root = folderBrowserDialog1.RootFolder;
            }
        }

        private void btnOpenOutputFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1 = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtOutputFolder.Text = folderBrowserDialog1.SelectedPath;
                //Environment.SpecialFolder root = folderBrowserDialog1.RootFolder;
            }
        }

        public static Image GenerateImage(Image backImage, Image frontImage)
        {
            int targetHeight = 512; //height and width of the finished image
            int targetWidth = 512;

            //be sure to use a pixelformat that supports transparency
            using (var bitmap = new Bitmap(targetWidth, targetHeight, PixelFormat.Format32bppArgb))
            {
                using (var canvas = Graphics.FromImage(bitmap))
                {
                    //this ensures that the backgroundcolor is transparent
                    canvas.Clear(Color.Transparent);

                    //this selects the entire backimage and and paints
                    //it on the new image in the same size, so its not distorted.
                    canvas.DrawImage(backImage,
                              new Rectangle(0, 0, backImage.Width, backImage.Height),
                              new Rectangle(0, 0, backImage.Width, backImage.Height),
                              GraphicsUnit.Pixel);

                    //this paints the frontimage with a offset at the given coordinates
                    canvas.DrawImage(frontImage, 0, 0);

                    canvas.Save();
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // in this case i needed to return a webimage,
                    //but you could simply save it to disk, or return a byte array.
                    //Again, be sure to save in a format that supports transparency,
                    //such as Png does.
                    bitmap.Save(memoryStream, ImageFormat.Png);
                    return new Bitmap(memoryStream);
                }
            }
        }

        public List<LayerInformation> Layers { get; set; } = new();
        public class LayerInformation
        {
            public LayerInformation(int Order, string Location, bool Selected, string DisplayText)
            {
                this.Order = Order;
                this.Location = Location;
                this.Selected = Selected;
                this.DisplayText = DisplayText;
            }
            public int Order { get; set; }
            public string Location { get; set; }
            public bool Selected { get; set; } = true;
            public string DisplayText { get; set; }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string layer1 = @"C:\\bitmap\Background\\Black#1.png";
                string layer2 = @"C:\\bitmap\\Layer1\\Middle#40.png";

                Image l1 = Image.FromFile(layer1);
                Image l2 = Image.FromFile(layer2);

                var res = GenerateImage(l1, l2);
                res.Save(@"C:\\bitmap\\res1.png");
                res.Dispose();

                Console.WriteLine("Finished");
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region helpers
        
        #endregion
    }
}
