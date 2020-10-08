using System;
using AxGrid.Base;
using SmartFormat;
using UnityEngine;

namespace AxGrid.Tools.Binders
{
    /// <summary>
    /// Биндит Model.SmartFormat к полю
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class UITextDataBind : Binder
    {
        [Header("События")]
        [Tooltip("Поля при изменении которых будет срабатывать собятие.")]
        public string[] fieldNames = new string[0];
        [Tooltip("Изменение люиого поля модели")]
        public bool modelChanged = true;
        
        [Header("Форматироване")]
        [Tooltip("Smart.Format(format, model)")]
        public string format = "{Balance.Game}";

        [Tooltip("Взять формат перед выводом")]
        public bool isFormatField = false;
        public bool applyModelForFromatField = true;
        private UnityEngine.UI.Text uiText;

        [OnAwake]
        void awake()
        {
            try
            {
                uiText = GetComponent<UnityEngine.UI.Text>();
            }
            catch (Exception e)
            {
                Log.Error("Error get Component:{0}", e.Message);
            }
        }
        
        [OnStart]
        public virtual void start()
        {
            try
            {
                if (isFormatField)
                    if (applyModelForFromatField)
                        format = Smart.Format(Text.Get(format), Model);
                    else
                        format = Text.Get(format);
                if (modelChanged)
                   Model.EventManager.AddAction("ModelChanged", Changed);
                else
                    foreach (var fieldName in fieldNames)
                       Model.EventManager.AddAction($"On{fieldName}Changed", Changed);
                Changed();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        [OnDestroy]
        public void onDestroy()
        {
            try
            {
                if (modelChanged)
                   Model.EventManager.RemoveAction("ModelChanged", Changed);
                else
                    foreach (var fieldName in fieldNames)
                       Model.EventManager.RemoveAction($"On{fieldName}Changed", Changed);
            }catch(Exception) {}
        }

        
        protected void Changed()
        {
            //Log.Info($"Value of [{fieldNames.Aggregate((a,b) => a+","+b)}] changed to {Txt.Get(format, MainSettings.Client.Model)}");
            uiText.text = Text.Get(format, Model);
        }
    }
}