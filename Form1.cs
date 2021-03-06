using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeterminantCalculator
{
    public partial class Form1 : Form
    {
        const int INPUT_FIELD_WIDTH = 40; // դաշտի չափսերը 
        const int INPUT_FIELD_HEIGHT = 26;  // դաշտի չափսերը
        const int DEFUALT_TOP_MARGIN = 50; // վերևից հեռավորությունը
        const int PADDING = 10; // դաշետրի միջև տարածությունը
        public Panel elementsPanel; // բոլոր դաշտերը սրա վրա են լինելու
        public TextBox[,] elementsTextBoxes; // դաշտեր
        public Button calculateButton; // button հաշվելու համար
        public Label resultLabel; // արդյունքը տեսնելու համար
        public double[,] elements; // դաշտերի արժեքները հաշվելու համար
        public int matrixSize; // մատրիցի չափսերը
        public Form1()
        {
            InitializeComponent();
            // կոմբոբոքսում ավելացնում ենք 2-ից 10 չափսերը
            for (int i = 2; i <= 10; ++i)
            {
                sizeSelector.Items.Add(i);
            }
            sizeSelector.SelectedItem = 3;
        }
        
        
        private void SizeSelector_SelectedIndexChanged(object sender, EventArgs e) // ջնջում է հին մատրիցը և սարքում նորը, երբ չափսը փոխում ենք
        {
            matrixSize = (int)sizeSelector.SelectedItem;
            Controls.Remove(elementsPanel);
            Controls.Remove(calculateButton);
            Controls.Remove(resultLabel);
            Controls.Remove(elementsPanel);
            elementsTextBoxes = new TextBox[matrixSize, matrixSize];
            if (matrixSize > 2)
            {
                this.Size = new Size(
                matrixSize * INPUT_FIELD_WIDTH + (matrixSize + 3) * PADDING,
                (matrixSize + 5) * INPUT_FIELD_HEIGHT + (matrixSize + 1) * PADDING + DEFUALT_TOP_MARGIN
                );
            }
            else
            {
                this.Size = new Size(
                3 * INPUT_FIELD_WIDTH + (3 + 3) * PADDING,
                (3 + 5) * INPUT_FIELD_HEIGHT + (3 + 1) * PADDING + DEFUALT_TOP_MARGIN
                );
            }
            elementsPanel = new Panel
            {
                Size = new Size(
                        matrixSize * INPUT_FIELD_WIDTH + (matrixSize + 1) * PADDING,
                        matrixSize * INPUT_FIELD_HEIGHT + (matrixSize + 1) * PADDING
                ),
                Location = new Point(0, DEFUALT_TOP_MARGIN)
            };
            Controls.Add(elementsPanel);

            //Debug.WriteLine(matrixSize);
            elements = new double[matrixSize, matrixSize];
            for (int i = 0; i < matrixSize; ++i)
            {
                for (int j = 0; j < matrixSize; ++j)
                {
                    TextBox t = new TextBox
                    {

                        Font = new Font("Microsoft Sans Serif", 12),
                        Text = "0",
                        Size = new Size(INPUT_FIELD_WIDTH, INPUT_FIELD_HEIGHT),
                        Location = new Point(
                            PADDING * (j + 1) + INPUT_FIELD_WIDTH * j,
                            PADDING * (i + 1) + INPUT_FIELD_HEIGHT * i)
                    };
                    elementsTextBoxes[i, j] = t;
                    t.KeyPress += ElementTextBox_KeyPress;

                    t.Leave += ElementTextBox_Leave;
                    Debug.WriteLine(t.Location);
                    elementsPanel.Controls.Add(t);
                }
            }
            calculateButton = new Button
            {
                Size = new Size(100, 26),
                Text = "Calculate",
                Location = new Point(this.Size.Width - 130, this.Size.Height - 130)
            };
            calculateButton.Click += CalculateButtonClick;
            Controls.Add(calculateButton);

            resultLabel = new Label
            {
                Size = new Size(100, 26),
                Location = new Point(this.Size.Width - 130, this.Size.Height - 94),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            Controls.Add(resultLabel);
        }

        private void ElementTextBox_Leave(object sender, EventArgs e) // եթե դատարկ string գրենք փոխի 0-ի
        {
            if((sender as TextBox).Text == "")
            {
                (sender as TextBox).Text = "0";
            }
        }

        private void CalculateButtonClick(object sender, EventArgs e) // հաշվում է դետերմինանտը
        {
            for(int i = 0; i < matrixSize; ++i)
            {
                for (int j = 0; j < matrixSize; ++j)
                {
                    elements[i, j] = double.Parse(elementsTextBoxes[i, j].Text);
                }
            }
            double determinant = MatrixMethods.Determinant(elements);
            resultLabel.Text = determinant.ToString();
        }
        private void ElementTextBox_KeyPress(object sender, KeyPressEventArgs e) // թույլ չի տալիս սխալ input ստանալ
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.Contains('.')))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '-') && ((sender as TextBox).Text.Contains('-')))
            {
                e.Handled = true;
            }
        }
    }
}
