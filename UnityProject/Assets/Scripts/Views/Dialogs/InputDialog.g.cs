using System;
using System.ComponentModel;
using UIData;


    partial class InputDialog : OcCommon.IDataContextType
    {
        Type OcCommon.IDataContextType.DataContextType { get { return typeof(InputDialogModel); } }
        public InputDialogModel ViewModel { get { return (InputDialogModel)DataContext; } }

        protected override void SetData(object source)
        {
            if (source == null || this == null) return;
            var data = source as InputDialogModel;
            if (data == null)
                throw new InvalidCastException(source.GetType().ToString() + " can't cast InputDialogModel");
            base.SetData(source);
            SetData(data);
        }

        private void SetData(InputDialogModel data)
        {
            OnDataInitializing(data);
            Title = data.Title;
            Input = data.Input;
            CanSubmit = data.CanSubmit;
            OnDataInitialized(data);
        }

        partial void OnDataInitializing(InputDialogModel data);
        partial void OnDataInitialized(InputDialogModel data);

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);
            var data = DataContext as InputDialogModel;
            if(data == null) return;
            if (e.PropertyName == "Input") data.Input = Input;
        }

        protected override void SourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.SourcePropertyChanged(sender, e);
            var data = DataContext as InputDialogModel;
            if(data == null) return;
            if (e.PropertyName == "Title") Title = data.Title;
            if (e.PropertyName == "Input") Input = data.Input;
            if (e.PropertyName == "CanSubmit") CanSubmit = data.CanSubmit;
        }
    }
