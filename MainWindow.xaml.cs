using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cybersecurity_chatbot_p2
{//start of namespace
    public partial class MainWindow : Window
    {//start of class

        //creating an instance of ArrayList
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();


        //variables to store the last detected topic for follow-up questions
        private string last_topic = "";
        private string username = "";

        public MainWindow()
        {//start of constructor
            InitializeComponent();

            //creating an instance of the voice_greeting class without an object name
            new voice_greeting();

            //creating an instance of the respond class 
            new respond(reply, ignore) { };



        }//end of constructor

        private void proceed(object sender, RoutedEventArgs e)
        {//start of method

        }//end of method

        private void submit_name(object sender, RoutedEventArgs e)
        {//start of method

        }//end of method

    }//end of class

}//end of namespace
