//===================================================
// Tyler Sriver
// Connect 4 Game - Form Class
// October 31, 2014
//===================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Connect4
{
    public partial class Form1 : Form
    {
        //instance of Game
        Game game1 = new Game();
        //List of Games to save and redraw with
        List<Game> pieces;

        //constructor of the Form
        public Form1()
        {
            InitializeComponent();
            pieces = new List<Game>();
        }

        //Method to handle painting of the form
        //handles repainting
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            game1.drawBoard(e);
            foreach (Game piece in pieces)
            {                
                 piece.redrawGamePiece(e.Graphics);              
            }   
            
        }

        //Method to handle mouse click in the panel
        //handles drawing the pieces, and checking win
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Color pcolor = new Color();
            Game piece = new Game(e.X, e.Y, pcolor);
            using (Graphics f = this.panel1.CreateGraphics())
            {
                game1.drawGamePiece(e, f);
                if(game1.player1)
                {
                    lblTurn.ForeColor = Color.Red;
                    lblTurn.Text = "Player 1's Turn";
                    pcolor = Color.Black;
                    pieces.Add(piece);
                }
                else
                {
                    lblTurn.ForeColor = Color.Black;
                    lblTurn.Text = "Player 2's Turn";
                    pcolor = Color.Red;
                    pieces.Add(piece);
                }
                
            }

            if (game1.WinningPlayer() == Color.Red)
            {
                MessageBox.Show("Red Player Wins", "Red Beat Black", MessageBoxButtons.OK);
                game1.Reset();
                panel1.Invalidate();
            }
            else if (game1.WinningPlayer() == Color.Black)
            {
                MessageBox.Show("Black Player Wins", "Black Beat Red", MessageBoxButtons.OK);
                game1.Reset();
                panel1.Invalidate();
            }
            
        }

        //Method for reset button, resets game when clicked
        private void btnReset_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult result;

            result = MessageBox.Show("Are you sure you want to reset?", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                panel1.Invalidate();
                game1.Reset();
                lblTurn.ForeColor = Color.Red;
                lblTurn.Text = "Player 1's Turn";
            }
            

        }

        //Method for saving the game
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter fileWriter;
            DialogResult result;
            string fileName;

            using (SaveFileDialog fileChooser = new SaveFileDialog())
            {
                fileChooser.CheckFileExists = false;
                fileChooser.AddExtension = true;
                fileChooser.DefaultExt = ".txt";
                fileChooser.Filter = "Text Files (*.txt)|*.txt";
                result = fileChooser.ShowDialog();
                fileName = fileChooser.FileName;
            }

            if (result == DialogResult.OK)
            {
                if (fileName == String.Empty)
                    MessageBox.Show("There is no name for that File.  You need to name your File.", "Give a File Name", MessageBoxButtons.OK, MessageBoxIcon.Error);

                else
                {

                    FileStream OutputFile = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    fileWriter = new StreamWriter(OutputFile);
                    foreach (var piece in pieces)
                        fileWriter.WriteLine(piece.ToString());
                    fileWriter.Close();
                    OutputFile.Close();
                }
            }
        }

        //Method for opeing the game
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName;
            DialogResult Result;
            StreamReader fileReader;

            using(OpenFileDialog fileChooser = new OpenFileDialog())
            {
                Result= fileChooser.ShowDialog();
                fileName = fileChooser.FileName;     
            }
            if(fileName == string.Empty)
                    MessageBox.Show("You must have a name for the file", "Please insert File Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try
                {
                        
                     pieces = new List<Game>();
                        
                     FileStream input = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                     fileReader = new StreamReader(input);
                     string FileLine;
                        
                     do
                     {
                           
                        FileLine = fileReader.ReadLine();
                            
                        if (FileLine != null)
                        {
                                
                            string[] parts = FileLine.Split(',');                               
                           // Game temporary = new Game(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), Color.FromName(parts[3]));                                
                            //pieces.Add(temporary);                                
                        }
                      }while (FileLine != null);
                        
                        panel1.Invalidate();                       
                        input.Close();
                        fileReader.Close();
                }                  
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"You chose the wrong file. " , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }   
            }  
        }     
        
    }
}
