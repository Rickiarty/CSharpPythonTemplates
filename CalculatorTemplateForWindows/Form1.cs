using System.Diagnostics;
using System.Configuration;
using System;
using System.Runtime.InteropServices;

namespace CalculatorTemplateForWindows
{
    public partial class Form1 : Form
    {
        [DllImport("HaskellLib\\Operation.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void hs_init(IntPtr argc, IntPtr argv);

        [DllImport("HaskellLib\\Operation.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void hs_exit();

        [DllImport("HaskellLib\\Operation.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int operation(int num);

        protected string pathToInterpreterForPython = "(Set this field in your personal settings in the 'AppConfig.settings' file.)";
        protected string pathToPythonLib = "(Set this field in your personal settings in the 'AppConfig.settings' file.)";
        protected string pathToOutputFile = "(Set this field in your personal settings in the 'AppConfig.settings' file.)";
        protected string operationX = "";
        protected float op1 = 0;
        protected float op2 = 0;
        protected bool proceeded = false;

        public Form1()
        {
            InitializeComponent();
            
            try
            {
                Console.WriteLine("Initializing runtime...");
                hs_init(IntPtr.Zero, IntPtr.Zero);

                Console.WriteLine("Calling to Haskell...");
                int index = 6;
                int answer = operation(index);
                Console.WriteLine("The {0}th number in Fibonnacci sequence is {1} by Haskell lib..", index, answer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //this.pathToInterpreterForPython = ConfigurationManager.AppSettings["pathToInterpreterForPython"];
            //this.pathToPythonLib = ConfigurationManager.AppSettings["pathToPythonLib"];
            //this.pathToOutputFile = ConfigurationManager.AppSettings["pathToOutputFile"];
            pathToInterpreterForPython = "\\Python\\Python310\\python.exe";
            pathToPythonLib = "\\CalculatorTemplateForWindows\\CalculatorTemplateForWindows\\PythonLib\\abstract_calculator.py";
            pathToOutputFile = "\\CalculatorTemplateForWindows\\CalculatorTemplateForWindows\\Output\\output.txt";
            this.proceeded = true;

            try
            {
                // Write to a file 
                File.WriteAllText(this.pathToOutputFile, "", System.Text.Encoding.UTF8); // Write an empty string to the file. 
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dResult = MessageBox.Show("Do you want to quit the app 'Calculator'?", "Quit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dResult == DialogResult.No) { e.Cancel = true; }
            else 
            {
                try
                {
                    hs_exit();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        protected string normalizeDecimalValueString(string str)
        {
            if (str == "") return "0.0";

            string[] arrStr = str.Split('.');
            if (arrStr.Length > 2) 
            { 
                return "0.0"; 
            }
            else if (arrStr.Length == 2)
            {
                if (arrStr[0] == "")
                {
                    return "0" + str;
                }
                else if (arrStr[1] == "")
                {
                    return str + "0";
                }
            }
            else
            { // arrStr.Length < 2 
                return str;
            }

            float tNum;
            if (!float.TryParse(str, out tNum))
            {
                return "0.0";
            }
            else 
            {
                return str;
            }
        }

        protected void runCmd()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = this.pathToInterpreterForPython;
            start.Arguments = string.Format("{0} {1}", this.pathToPythonLib, string.Format("{0} {1} {2}", this.operationX, this.op1, this.op2));
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }

            // Read from a file 
            string text = File.ReadAllText(this.pathToOutputFile); // Read all the content of the file. 
            this.textBox1.Text = text;
        }

        protected void calcResult()
        {
            this.textBox1.Text = this.normalizeDecimalValueString(this.textBox1.Text);
            this.op2 = float.Parse(this.textBox1.Text);
            this.textBox1.Text = "";
            this.proceeded = true;
            this.runCmd();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // = 
            this.calcResult();
        }

        protected void setoperationX(string operationX)
        {
            this.textBox1.Text = this.normalizeDecimalValueString(this.textBox1.Text);
            this.op1 = float.Parse(this.textBox1.Text);
            this.textBox1.Text = "";
            this.operationX = operationX;
            this.proceeded = true;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            // ÷ 
            this.setoperationX("/");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            // × 
            this.setoperationX("*");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            // － 
            this.setoperationX("-");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // ＋ 
            this.setoperationX("+");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            // operation executed by Python 
            // [operationX X] 
            // to customize your project for difference from other's 
            //this.setoperationX("X"); // operationX X 

            // operation executed by Haskell 
            this.textBox1.Text = this.normalizeDecimalValueString(this.textBox1.Text);
            int result = operation((int)float.Parse(this.textBox1.Text)); // execute operation from DLL imported 
            this.textBox1.Text = result.ToString();
        }

        protected void checkProceeded()
        {
            if (this.proceeded)
            {
                this.textBox1.Text = "";
                this.proceeded = false;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // 0 
            this.checkProceeded();
            this.textBox1.Text += '0'; // append last 
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // 1 
            this.checkProceeded();
            this.textBox1.Text += '1'; // append last 
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // 2 
            this.checkProceeded();
            this.textBox1.Text += '2'; // append last 
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // 3 
            this.checkProceeded();
            this.textBox1.Text += '3'; // append last 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 4 
            this.checkProceeded();
            this.textBox1.Text += '4'; // append last 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // 5 
            this.checkProceeded();
            this.textBox1.Text += '5'; // append last 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // 6 
            this.checkProceeded();
            this.textBox1.Text += '6'; // append last 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 7 
            this.checkProceeded();
            this.textBox1.Text += '7'; // append last 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 8 
            this.checkProceeded();
            this.textBox1.Text += '8'; // append last 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 9 
            this.checkProceeded();
            this.textBox1.Text += '9'; // append last 
        }

        private void button17_Click(object sender, EventArgs e)
        {
            // . 
            this.checkProceeded();
            if (!this.textBox1.Text.Contains('.') && this.textBox1.Text != "") // The string in textBox1 is NOT empty. And there is NO decimal point(小數點) in the string in textBox1. 
            {
                this.textBox1.Text += '.'; // append last 
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            // 'Clear' button pressed 
            this.clear();
        }

        protected void clear()
        {
            this.op1 = 0;
            this.op2 = 0;
            this.operationX = "";
            this.textBox1.Text = "";
            this.proceeded = false;
        }

    }
}
