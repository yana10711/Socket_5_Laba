using System;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Net;
namespace try2
{
    public partial class KNB : Form
    {
        int port = 12345;
        StreamReader reader;
        StreamWriter writer;
        bool sent_hand = false;
        bool read_hand = false;
        string my_hand = "";
        string op_hand = "";

        public KNB()
        {
            InitializeComponent();
            setButtons(false);
        }
        private void setButtons(bool enable)
        {
            button1.Enabled = enable;
            button2.Enabled = enable;
            button3.Enabled = enable;


        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
                StartServer();
            if (radioButton2.Checked)
                StartClient();
            setButtons(true);
            timer1.Enabled = true;
        }
        private void StartServer()
        {
            TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Parse(textBox2.Text), port));
            listener.Start();
            TcpClient server = listener.AcceptTcpClient();
            server.ReceiveTimeout = 50;
            reader = new StreamReader(server.GetStream());
            writer = new StreamWriter(server.GetStream());
            writer.AutoFlush = true;

        }
        private void StartClient()
        {
            TcpClient client = new TcpClient();
            client.Connect(textBox2.Text, port);
            client.ReceiveTimeout = 50;
            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            send("Камень");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            send("Ножницы");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            send("Бумага");

        }
        private void send(string text)
        {
            if (sent_hand) return;
            writer.WriteLine(text);
            sent_hand = true;
            my_hand = text;
            setButtons(false);
           // textBox1.Text += "send" + text + Environment.NewLine;
        }
        private string read()
        {
            if (read_hand) return "";
            try
            {
                string text;
                text = reader.ReadLine();
                read_hand = true;
                op_hand = text;
              //  textBox1.Text += "read" + text + Environment.NewLine;
                return text;
            }
            catch
            {
                return "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           /* textBox1.Text =
               "sent_hand =" + sent_hand.ToString() + Environment.NewLine +
               "read_hand =" + read_hand.ToString() + Environment.NewLine +
               "my_hand =" + my_hand + Environment.NewLine + "op_hand =" + op_hand; */
            string hand = read();
            if (sent_hand && read_hand)
            {
                int ch = CompareHands(my_hand, op_hand);
                string res = "";
                if (ch == 0) res = "Ничья";
                if (ch == 1) res = "Победа";
                if (ch == 2) res = "Проигрыш";
                label2.Text = my_hand +  " "+ "VS"+" "+ op_hand + " " + "-" + " " + res;
                sent_hand = false;
                 read_hand = false;
                setButtons(true);
                my_hand = "";
                op_hand = "";
            }
        }
        private int CompareHands(string hand1, string hand2)
        {
            if (hand1 == hand2) return 0;
            if (hand1 == "Камень")
                if (hand2 == "Ножницы")
                    return 1;
                else return 2;

            if (hand1 == hand2) return 0;
            if (hand1 == "Ножницы")
                if (hand2 == "Бумага")
                    return 1;
                else return 2;

            if (hand1 == hand2) return 0;
            if (hand1 == "Бумага")
                if (hand2 == "Камень")
                    return 1;
                else return 2;
            return 0;

        }
    }
}
