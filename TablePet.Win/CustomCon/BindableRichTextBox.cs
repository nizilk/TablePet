using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Runtime.Remoting.Channels;

namespace TablePet.Win.CustomCon
{
    public class BindableRichTextBox : RichTextBox
    {
        public FlowDocument BindableDocument
        {
            get
            {
                return (FlowDocument)GetValue(DocumentProperty);
            }
            set
            {
                SetValue(DocumentProperty, value);
            }
        }
        // Using a DependencyProperty as the backing store for Document.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("BindableDocument", typeof(FlowDocument), typeof(BindableRichTextBox), new UIPropertyMetadata(null, new PropertyChangedCallback(OnDocumentChanged)));
        private static void OnDocumentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            BindableRichTextBox textBox = sender as BindableRichTextBox;
            if (textBox != null)
            {
                textBox.changeFromBinding = true;
                textBox.OnDocumentPropertyChanged(e);
            }
        }

        private bool changeFromBinding = false;

        protected virtual void OnDocumentPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (changeFromBinding)
            {
                this.Document = e.NewValue as FlowDocument;
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (!changeFromBinding)
            {
                this.BindableDocument = this.Document;
            }

            changeFromBinding = false;
        }
    }
}
