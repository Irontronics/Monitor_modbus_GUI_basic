using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


namespace testing1
{
    public partial class Form1 : Form
    {

        string dato;      //para los datos 
        sbyte index0fZ, index0fY, index0fX, index0fW, index0fV;
        String dataMod1, dataMod2, dataMod3, dataMod4, dataMod5;
        bool StatusButton = true; 

        public Form1()
        {
            InitializeComponent();
            string[] puertos = SerialPort.GetPortNames(); //obtener puertos disponibles en pc

            foreach (string mostrar in puertos) //mostrar en combobox 
            {
                comboBox1.Items.Add(mostrar);

            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
        
                try
                {
                if (StatusButton)
                {
                    serialPort1.Write("A2$");
                    button2.Text = "Axis Disable";
                    StatusButton = false;
                    
                }
                else {
                    serialPort1.Write("B2$");
                    button2.Text = "Axis Enable";
                    StatusButton = true;
                }

                }
                catch (Exception error)
                {

                    MessageBox.Show(error.Message);
                } 
            }

    

        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.Close(); //revisar que este cerrado el puerto
            serialPort1.Dispose();
            string puertoseleccionado = comboBox1.Text;
            serialPort1.PortName = puertoseleccionado;
            serialPort1.Open();
            CheckForIllegalCrossThreadCalls = false;
            if (serialPort1.IsOpen)
            {

                label10.Text = "Esta abierto";
            }
            else {
                return; 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            serialPort1.Dispose();
            Close();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e) // cuando se recibe un dato de arduino a Visual studio ! 
        {
            dato = serialPort1.ReadLine(); //INICO DE COMUNICACIÓN 
            this.BeginInvoke(new EventHandler(ProcessData));




        }

        private void ProcessData(object sender, EventArgs e)
        {
            try
            {

                index0fZ = Convert.ToSByte(dato.IndexOf("Z"));
                index0fY = Convert.ToSByte(dato.IndexOf("Y"));
                index0fX = Convert.ToSByte(dato.IndexOf("X"));
                index0fW = Convert.ToSByte(dato.IndexOf("W"));
                index0fV = Convert.ToSByte(dato.IndexOf("V"));


                dataMod1 = dato.Substring(0, index0fZ);
                dataMod2 = dato.Substring(index0fZ + 1, (index0fY - index0fZ) - 1);
                dataMod3 = dato.Substring(index0fY + 1, (index0fX - index0fY) - 1 );
                dataMod4 = dato.Substring(index0fX + 1, (index0fW - index0fX) - 1);
                dataMod5 = dato.Substring(index0fW + 1, (index0fV - index0fW) - 1);

                double numero1 = Convert.ToDouble(dataMod1); //convertir dato a double 
                double numero2 = Math.Round(((numero1 * 245735) / 4294967295), 2);
                if (numero2 > 10) { label9.Text = numero2.ToString(); } else { label9.Text = "0";  }
                
                if (Convert.ToString(dataMod2) == "1") { label11.BackColor = Color.Green; } else if (Convert.ToString(dataMod2) == "0") { label11.BackColor = Color.Red; }
                if (Convert.ToString(dataMod3) == "1") { label12.BackColor = Color.Green; } else if (Convert.ToString(dataMod3) == "0") { label12.BackColor = Color.Red; }

                double numero3 = Convert.ToDouble(dataMod4); //convertir dato a double 
                double numero4 = numero3 / 1000;
                numero4 = Math.Round(numero4, 2);
                label13.Text = numero4.ToString();

                label14.Text = Convert.ToString(dataMod5); 



            }
            catch(Exception error) {

                MessageBox.Show(error.Message); 
            
            }


        }
    }
}
