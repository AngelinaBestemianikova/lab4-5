
using System.Windows;
using System.Windows.Controls;

namespace lab4_5
{
    public class MyButton : Button
    {
        static MyButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyButton), new FrameworkPropertyMetadata(typeof(MyButton)));
        }
    }
}
