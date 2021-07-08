using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RedDotUI : MonoBehaviour
{
    public string path;
    public Text   text;
    public Button button;
    
    void Start()
    {
        button = GetComponent<Button> ();
        button.onClick.AddListener((() =>
        {
            int value = RedDotManager.instance.GetValue(path);
            RedDotManager.instance.ChangeValue(path, value + 1);
        }));
        TreeNode node = RedDotManager.instance.AddListener(path, ReddotCallback);
        gameObject.name = node.fullPath;
    }

    private void ReddotCallback(int value)
    {
        Debug.Log("红点刷新，路径:" + path + ",当前帧数:" + Time.frameCount + ",值:" + value);
        text.text = value.ToString();
    }
}
