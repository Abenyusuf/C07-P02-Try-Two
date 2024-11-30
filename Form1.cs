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

namespace C07_P02
{
    public partial class Form1 : Form
    {
        private string[] correctAnswers;

        public Form1()
        {
            InitializeComponent();
        }

        private void ReadCorrect(List<string> AnswerList)
        {
            try
            {
                StreamReader inputFile = File.OpenText("Correct.txt");
                //  Open the file
                while (!inputFile.EndOfStream)
                //  Continue processing until the end of the file is reached
                {
                    string Line = inputFile.ReadLine();
                    if (string.IsNullOrEmpty(Line))
                    {
                        continue;
                    }
                    // Add the line to the list
                    AnswerList.Add(Line);
                }
                inputFile.Close();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("The file 'Correct.txt' was not found.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayAnswers(List<string> AnswerList)
        {
            OutputlistBox.Items.Clear(); // Clear previous items
            foreach (string value in AnswerList)
            {
                OutputlistBox.Items.Add(value);
                //  Display the sales values
            }
        }

        private void InputAnswers(List<string> studentList)
        {
            for (int i = 1; i <= 20; i++) // Loop through 20 questions
            {
                // Dynamically find the GroupBox for the current question
                Control[] controls = this.Controls.Find($"Question{i}Groupbox", true);

                if (controls.Length > 0 && controls[0] is GroupBox groupBox)
                {
                    string selectedAnswer = null;

                    // Check each RadioButton inside the GroupBox
                    foreach (Control control in groupBox.Controls)
                    {
                        if (control is RadioButton radioButton && radioButton.Checked)
                        {
                            selectedAnswer = radioButton.Text; // Get the text of the selected RadioButton
                            break;
                        }
                    }

                    if (selectedAnswer != null)
                    {
                        studentList.Add(selectedAnswer);
                    }
                    else
                    {
                        // If no answer is selected, show an error message
                        MessageBox.Show($"Please select an answer for question {i}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show($"GroupBox for question {i} not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void CompareAnswers(List<string> AnswerList, List<string> StudentList)
        {
            int correctCount = 0;
            List<int> incorrectQuestions = new List<int>();

            for (int i = 0; i < AnswerList.Count; i++)
            {
                if (i < StudentList.Count && AnswerList[i] == StudentList[i])
                {
                    correctCount++;
                }
                else
                {
                    incorrectQuestions.Add(i + 1);
                }
            }

            // Determine pass or fail
            bool passed = correctCount >= 15;
            string resultMessage = passed ? "Passed" : "Failed";

            // Display results
            LabelResults.Text = ($@"
            Result: {resultMessage}
            Correct Answers: {correctCount}
            Incorrect Answers: {incorrectQuestions.Count}
            Incorrect Questions: {string.Join(", ", incorrectQuestions)}
            ");
        }


        private void CheckButton_Click_1(object sender, EventArgs e)
            {
                List<string> AnswerList = new List<string>();
                List<string> StudentList = new List<string>();
                ReadCorrect(AnswerList);
                DisplayAnswers(AnswerList);
                InputAnswers(StudentList);
                CompareAnswers(AnswerList, StudentList);
           

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
    }
    }

