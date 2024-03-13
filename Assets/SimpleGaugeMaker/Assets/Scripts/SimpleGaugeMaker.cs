using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum PositionAlign
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
    Center
}

[Serializable]
public enum DialAlign
{
    TopLeft,
    BottomRight
}

[Serializable]
public class NeedleLimiter
{
    public String LimiterName = "unnamed";
    public Vector2 minMaxValue = new Vector2(0, 100);
    public Vector2 StartEndValue = new Vector2(0, 100);
}

[Serializable]
public class Knots
{
    public String Name = "KnotUnnamed";
    
    public PositionAlign Alignement = PositionAlign.Center;
    public Texture2D Texture = null;
    public Vector2 Position = Vector2.zero;

    [Range(0.1f, 2.0f)]
    public float ScaleRatio = 1.0f;
    public bool DialRatioLock = false;
    public bool AlignToNeedle = false;
    public String NeedleTag = "none";
    public bool Hide = false;
}

[Serializable]
public class Needles
{
    public String tagName = "Input";
    public PositionAlign Alignement = PositionAlign.Center;
    public Texture2D Texture = null;
    public Vector2 Pivot = Vector2.zero;
    public float PivotOffset = 0.0f;

    [HideInInspector]
    public Vector2 posPivot = Vector2.zero;

    [Range(.1f, 2.0f)]
    public float ScaleRatio = 1.0f;
    public bool DialRatioLock = false;

    public NeedleLimiter[] needleLimiter = null;

    public bool Hide = false;
}

[Serializable]
public class Dial
{
    public DialAlign Alignement = DialAlign.TopLeft;
    public Texture2D Texture = null;
    public Vector2 Position = new Vector2(0, 0);

    [Range(.1f, 2.0f)]
    public float ScaleRatio = 0.7f;
}

[Serializable]
public class GaugeInputValues
{
    public string tagName = "input";
    public Vector2 minMaxValue = new Vector2(0, 100);
    public float value = 0f;
}

[Serializable]
public class TextLayer
{
    public string tagName = "input";
    public Color Color = Color.black;
    public Font Font = null;
    public int FontSize = 14;
    public FontStyle fontStyle = FontStyle.Normal;
    public PositionAlign Alignement = PositionAlign.Center;
    public Vector2 Position = Vector2.zero;
    public Vector2 Size = new Vector2(100, 50);
    public bool useTagValue = false;
    public bool roundToInt = false;
}

[AddComponentMenu("Noir Project/Simple Gauge Maker/Gauge")]
[ExecuteInEditMode]

public class SimpleGaugeMaker : MonoBehaviour 
{
    public bool Hide = false;

    public GaugeInputValues[] gaugeInputs = null;
    public Dial dial = null;
    public Needles[] needles = null;
    public Knots[] knots = null;
    public TextLayer[] textLayer = null;
	
    //this will set the desired input by tagname
    public void setInputValue(String TagName, float value)
    {
        if (gaugeInputs.Length <= 0)
        {
            return;
        }

        foreach (GaugeInputValues g in gaugeInputs)
        {
            if (g.tagName == TagName)
            {
                g.value = value;
                return;
            }
        }
    }

    //this can set the Text Layer
    public void setTextLayerTag(int LayerIndex, String NewTagName, bool ValFromInput)
    {
        if (textLayer[LayerIndex] != null)
        {
            textLayer[LayerIndex].tagName = NewTagName;

            if (ValFromInput)
                textLayer[LayerIndex].useTagValue = true;
            else
                textLayer[LayerIndex].useTagValue = false;
        }
    }

    public int getTextLayerIndex(string TagName)
    {
        if (textLayer.Length > 0)
        {
            int indexCnt = 0;
            foreach (TextLayer t in textLayer)
            {
                if (t.tagName == TagName)
                {
                    return indexCnt;
                }
                indexCnt++;
            }
        }

        //when return -1, the tagname is not found.
        Debug.Log("Tagname for search index of Text Layer is not found!");
        return -1;
    }

	// Update is called once per frame
    void OnGUI()
    {
        if (!Hide)
        {

            //This is the order of gui layer too, the dial is back layer and the knot in now top layer

            //Dial | you should draw this one first because it is the first layer
            DialDrawer();

            //Text Layer
            TextDrawer();

            //Needle
            NeedleDrawer();

            //Gauge
            KnotDrawer();

            
        }
	}

    //TextLayer Drawer
    public void TextDrawer()
    {
        if (textLayer.Length > 0)
        {
            Color oldcolor = GUI.color;
            TextAnchor oldAnchor = GUI.skin.label.alignment;
            float posX = 0f;
            float posY = 0f;
            float cValue = 0f;

            foreach (TextLayer t in textLayer)
            {
                GUI.skin.label.font = t.Font;
                GUI.skin.label.fontSize = t.FontSize;
                GUI.skin.label.fontStyle = t.fontStyle;
                GUI.color = t.Color;

                posX = t.Position.x;
                posY = t.Position.y;

                // alignement of the dial is Top Left
                if (dial.Alignement == DialAlign.TopLeft)
                {
                    //The center alignment require dial defined
                    if (t.Alignement == PositionAlign.Center && dial.Texture != null)
                    {
                        posX = dial.Position.x + (dial.Texture.width * dial.ScaleRatio / 2) + t.Position.x - (t.Size.x / 2);
                        posY = dial.Position.y + (dial.Texture.height * dial.ScaleRatio / 2) + t.Position.y - (t.Size.y / 2);
                        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                    }

                    //Top left Align
                    if (t.Alignement == PositionAlign.TopLeft && dial.Texture != null)
                    {
                        posX = dial.Position.x + t.Position.x;
                        posY = dial.Position.y + t.Position.y;
                        GUI.skin.label.alignment = TextAnchor.UpperLeft;
                    }

                    //Top Right Align
                    if (t.Alignement == PositionAlign.TopRight && dial.Texture != null)
                    {
                        posX = (dial.Position.x + dial.Texture.width * dial.ScaleRatio) + t.Position.x - (t.Size.x);
                        posY = dial.Position.y + t.Position.y;
                        GUI.skin.label.alignment = TextAnchor.UpperRight;
                    }

                    //Bottom Left
                    if (t.Alignement == PositionAlign.BottomLeft && dial.Texture != null)
                    {
                        posX = dial.Position.x + t.Position.x;
                        posY = (dial.Position.y + dial.Texture.height * dial.ScaleRatio) + t.Position.y - t.Size.y;
                        GUI.skin.label.alignment = TextAnchor.LowerLeft;
                    }

                    //Bottom Right
                    if (t.Alignement == PositionAlign.BottomRight && dial.Texture != null)
                    {
                        posX = (dial.Position.x + dial.Texture.width * dial.ScaleRatio) + t.Position.x - t.Size.x;
                        posY = (dial.Position.y + dial.Texture.height * dial.ScaleRatio) + t.Position.y - t.Size.y;
                        GUI.skin.label.alignment = TextAnchor.LowerRight;
                    }

                }

                //when the dial alignment is bottom left some conditions are different
                if (dial.Alignement == DialAlign.BottomRight)
                {
                    //The center alignment require dial defined
                    if (t.Alignement == PositionAlign.Center && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x + dial.Texture.width * dial.ScaleRatio / 2) + t.Position.x - (t.Size.x / 2);
                        posY = Screen.height - (dial.Position.y + dial.Texture.height * dial.ScaleRatio / 2) + t.Position.y - (t.Size.y / 2);
                    }

                    //Top left Align
                    if (t.Alignement == PositionAlign.TopLeft && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x + dial.Texture.width * dial.ScaleRatio) + t.Position.x;
                        posY = Screen.height - (dial.Position.y + dial.Texture.height * dial.ScaleRatio) + t.Position.y;
                        GUI.skin.label.alignment = TextAnchor.UpperLeft;
                    }

                    //Top Right Align
                    if (t.Alignement == PositionAlign.TopRight && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x) + t.Position.x - t.Size.x;
                        posY = Screen.height - (dial.Position.y + dial.Texture.height * dial.ScaleRatio) + t.Position.y;
                        GUI.skin.label.alignment = TextAnchor.UpperRight;
                    }

                    //Bottom Left
                    if (t.Alignement == PositionAlign.BottomLeft && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x + dial.Texture.width * dial.ScaleRatio) + t.Position.x;
                        posY = Screen.height - (dial.Position.y) + t.Position.y - t.Size.y;
                        GUI.skin.label.alignment = TextAnchor.LowerLeft;
                    }

                    //Bottom Right
                    if (t.Alignement == PositionAlign.BottomRight && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x) + t.Position.x - t.Size.x;
                        posY = Screen.height - (dial.Position.y) + t.Position.y - t.Size.y;
                        GUI.skin.label.alignment = TextAnchor.LowerRight;
                    }
                }


                //Grab the value and draw on screen
                if (t.useTagValue)
                {
                    cValue = 0;
                    if (gaugeInputs.Length > 0)
                    {
                        foreach (GaugeInputValues g in gaugeInputs)
                        {
                            if (g.tagName == t.tagName)
                            {
                                cValue = g.value;

                                if (t.roundToInt)
                                    cValue = Mathf.RoundToInt(cValue);
                            }
                        }
                    }

                    //using tag value
                    GUI.Label(new Rect(posX, posY,
                           t.Size.x, t.Size.y),
                           cValue.ToString());
                } 
                else
                {
                    //When using tag name
                    GUI.Label(new Rect(posX, posY,
                           t.Size.x, t.Size.y),
                           t.tagName);
                }

                GUI.color = oldcolor;
                GUI.skin.label.alignment = oldAnchor;
            }
            
        }
        
       
        
    }

    //Draw needle for Gauge
    public void NeedleDrawer()
    {
        if (needles.Length > 0)
        {
            foreach (Needles n in needles)
            {
                //when the dial is in hide status go to next needle when available
                if (n.Hide) continue;

                //local values for storing current needle values from input manager
                //Vector2 minMaxValue = Vector2.zero;
                //Vector2 startEndAngle = Vector2.zero;
                float cValue = 0f;

                float posX = 0f;
                float posY = 0f;

                //When dial lock is enabled, replicate the dial scale ratio on needle
                if (n.DialRatioLock) n.ScaleRatio = dial.ScaleRatio;

                //Current GUI Matrix
                Matrix4x4 guiMatrix = GUI.matrix;

                // alignement of the dial is Top Left now
                if (dial.Alignement == DialAlign.TopLeft)
                {
                    //The center alignment require dial defined
                    if (n.Alignement == PositionAlign.Center && dial.Texture != null)
                    {
                        posX = (dial.Position.x + ((dial.Texture.width * dial.ScaleRatio / 2) - (n.Texture.width * n.ScaleRatio / 2))) + n.Pivot.x + n.Texture.height;
                        posY = (dial.Position.y + ((dial.Texture.height * dial.ScaleRatio / 2) - (n.Texture.height * n.ScaleRatio / 2))) + n.Pivot.y;
                    }

                    //Top left Align
                    if (n.Alignement == PositionAlign.TopLeft && dial.Texture != null)
                    {
                        posX = dial.Position.x + n.Pivot.x + (n.Texture.height / 2);
                        posY = dial.Position.y + n.Pivot.y + (n.Texture.height / 2);
                    }

                    //Top Right Align
                    if (n.Alignement == PositionAlign.TopRight && dial.Texture != null)
                    {
                        posX = (dial.Position.x + dial.Texture.width * dial.ScaleRatio) + n.Pivot.x - (n.Texture.height / 2);
                        posY = dial.Position.y + n.Pivot.y + (n.Texture.height / 2);
                    }

                    //Bottom Left
                    if (n.Alignement == PositionAlign.BottomLeft && dial.Texture != null)
                    {
                        posX = dial.Position.x + n.Pivot.x + (n.Texture.height / 2);
                        posY = (dial.Position.y + dial.Texture.height * dial.ScaleRatio) + n.Pivot.y - (n.Texture.height / 2);
                    }

                    //Bottom Right
                    if (n.Alignement == PositionAlign.BottomRight && dial.Texture != null)
                    {
                        posX = (dial.Position.x + dial.Texture.width * dial.ScaleRatio) + n.Pivot.x - (n.Texture.height / 2);
                        posY = (dial.Position.y + dial.Texture.height * dial.ScaleRatio) + n.Pivot.y - (n.Texture.height / 2);
                    }
                }

                //when the dial alignment is bottom left some conditions are different
                if (dial.Alignement == DialAlign.BottomRight)
                {
                    //The center alignment require dial defined
                    if (n.Alignement == PositionAlign.Center && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x + dial.Texture.width * dial.ScaleRatio / 2) - (n.Texture.width * n.ScaleRatio / 2) + n.Pivot.x + n.Texture.height;
                        posY = Screen.height - (dial.Position.y + dial.Texture.height * dial.ScaleRatio / 2) - (n.Texture.height * n.ScaleRatio / 2) + n.Pivot.y;
                    }

                    //Top left Align
                    if (n.Alignement == PositionAlign.TopLeft && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x + dial.Texture.width * dial.ScaleRatio) + n.Pivot.x + (n.Texture.height / 2);
                        posY = Screen.height - (dial.Position.y + dial.Texture.height * dial.ScaleRatio) + n.Pivot.y + (n.Texture.height / 2);
                    }

                    //Top Right Align
                    if (n.Alignement == PositionAlign.TopRight && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x) + n.Pivot.x - (n.Texture.height / 2);
                        posY = Screen.height - (dial.Position.y + dial.Texture.height * dial.ScaleRatio) + n.Pivot.y + (n.Texture.height / 2);
                    }

                    //Bottom Left
                    if (n.Alignement == PositionAlign.BottomLeft && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x + dial.Texture.width * dial.ScaleRatio) + n.Pivot.x + (n.Texture.height / 2);
                        posY = Screen.height - (dial.Position.y) + n.Pivot.y - (n.Texture.height / 2);
                    }

                    //Bottom Right
                    if (n.Alignement == PositionAlign.BottomRight && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x) + n.Pivot.x - (n.Texture.height / 2);
                        posY = Screen.height - (dial.Position.y) + n.Pivot.y - (n.Texture.height / 2);
                    }
                }

                //temporary record the new X and Y for further use
                n.posPivot = new Vector2(posX, posY);

                if (gaugeInputs.Length > 0)
                {
                    foreach (GaugeInputValues g in gaugeInputs)
                    {
                        if (g.tagName == n.tagName)
                        {
                            cValue = g.value;

                            if (cValue >= g.minMaxValue.y)
                            {
                                cValue = g.minMaxValue.y;
                                g.value = cValue;
                            }
                                

                            if (cValue <= g.minMaxValue.x)
                            {
                                cValue = g.minMaxValue.x;
                                g.value = cValue;
                            }
                                
                        }
                    }
                }

                //Draw Needle
                if (n.needleLimiter.Length > 0)
                {
                    float start = 0f;
                    float end = 0f;

                    float normalizedInput = 0;

                    foreach (NeedleLimiter nl in n.needleLimiter)
                    {
                        if (cValue >= nl.minMaxValue.x && cValue <= nl.minMaxValue.y)
                        {
                            start = nl.StartEndValue.x;
                            end = nl.StartEndValue.y;

                            normalizedInput = (cValue - nl.minMaxValue.x) / (nl.minMaxValue.y - nl.minMaxValue.x);

                        }
                    }
                    float angle = Mathf.Lerp(start, end, normalizedInput);

                    GUIUtility.RotateAroundPivot(angle, new Vector2(posX + n.PivotOffset, posY));
                    GUI.DrawTexture(new Rect(posX,
                                             posY - (n.Texture.height * n.ScaleRatio) / 2,
                                             n.Texture.width * (n.ScaleRatio),
                                             n.Texture.height * (n.ScaleRatio)),
                                             n.Texture);

                    GUI.matrix = guiMatrix;
                }
            }
        }
    }

    //this function will draw the Dial Panel
    public void DialDrawer()
    {
        //This will draw speedoMeter Background
        if (dial != null)
        {
            float posX = 0f;
            float posY = 0f;

            if (dial.Alignement == DialAlign.TopLeft)
            {
                posX = dial.Position.x;
                posY = dial.Position.y;
            }

            if (dial.Alignement == DialAlign.BottomRight)
            {
                posX = Screen.width - (dial.Position.x + dial.Texture.width * dial.ScaleRatio);
                posY = Screen.height - (dial.Position.y + dial.Texture.height * dial.ScaleRatio);
            }

            //Draw speedometer bg
            GUI.DrawTexture(
                    new Rect(
                        posX,
                        posY,
                        dial.Texture.width * (dial.ScaleRatio),
                        dial.Texture.height * (dial.ScaleRatio)),
                    dial.Texture);
        }
    }

    //this function draws all of the knots and called in ongui
    public void KnotDrawer()
    {
        if (knots.Length > 0)
        {
            foreach(Knots k in knots)
            {
                if (k.Hide)
                    continue;

                float posX = 0f;
                float posY = 0f;

                // alignement of the dial is Top Left now
                if (dial.Alignement == DialAlign.TopLeft)
                {
                    //The center alignment require dial defined
                    if (k.Alignement == PositionAlign.Center && dial.Texture != null)
                    {
                        posX = (dial.Position.x + ((dial.Texture.width * dial.ScaleRatio / 2) - (k.Texture.width * k.ScaleRatio / 2))) + k.Position.x;
                        posY = (dial.Position.y + ((dial.Texture.height * dial.ScaleRatio / 2) - (k.Texture.height * k.ScaleRatio / 2))) + k.Position.y;
                    }

                    //Top left Align
                    if (k.Alignement == PositionAlign.TopLeft && dial.Texture != null)
                    {
                        posX = dial.Position.x + k.Position.x;
                        posY = dial.Position.y + k.Position.y;
                    }

                    //Top Right Align
                    if (k.Alignement == PositionAlign.TopRight && dial.Texture != null)
                    {
                        posX = (dial.Position.x + dial.Texture.width * dial.ScaleRatio) + k.Position.x - k.Texture.width;
                        posY = dial.Position.y + k.Position.y;
                    }

                    //Bottom Left
                    if (k.Alignement == PositionAlign.BottomLeft && dial.Texture != null)
                    {
                        posX = dial.Position.x + k.Position.x;
                        posY = (dial.Position.y + dial.Texture.height * dial.ScaleRatio) + k.Position.y - k.Texture.height;
                    }

                    //Bottom Right
                    if (k.Alignement == PositionAlign.BottomRight && dial.Texture != null)
                    {
                        posX = (dial.Position.x + dial.Texture.width * dial.ScaleRatio) + k.Position.x - k.Texture.width;
                        posY = (dial.Position.y + dial.Texture.height * dial.ScaleRatio) + k.Position.y - k.Texture.height;
                    }
                }

                //when the dial alignment is bottom left some conditions are different
                if (dial.Alignement == DialAlign.BottomRight)
                {
                    //The center alignment require dial defined
                    if (k.Alignement == PositionAlign.Center && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x + dial.Texture.width * dial.ScaleRatio / 2) - (k.Texture.width * k.ScaleRatio / 2) + k.Position.x;
                        posY = Screen.height - (dial.Position.y + dial.Texture.height * dial.ScaleRatio / 2) - (k.Texture.height * k.ScaleRatio / 2) + k.Position.y;
                    }

                    //Top left Align
                    if (k.Alignement == PositionAlign.TopLeft && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x + dial.Texture.width * dial.ScaleRatio) + k.Position.x;
                        posY = Screen.height - (dial.Position.y + dial.Texture.height * dial.ScaleRatio) + k.Position.y;
                    }

                    //Top Right Align
                    if (k.Alignement == PositionAlign.TopRight && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x) + k.Position.x - k.Texture.width;
                        posY = Screen.height - (dial.Position.y + dial.Texture.height * dial.ScaleRatio) + k.Position.y;
                    }

                    //Bottom Left
                    if (k.Alignement == PositionAlign.BottomLeft && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x + dial.Texture.width * dial.ScaleRatio) + k.Position.x;
                        posY = Screen.height - (dial.Position.y) + k.Position.y - k.Texture.height;
                    }

                    //Bottom Right
                    if (k.Alignement == PositionAlign.BottomRight && dial.Texture != null)
                    {
                        posX = Screen.width - (dial.Position.x) + k.Position.x - k.Texture.width;
                        posY = Screen.height - (dial.Position.y) + k.Position.y - k.Texture.height;
                    }
                }

                //When the scale lock is activated, lock the knot scale ratio same as dial
                if (k.DialRatioLock)
                {
                    k.ScaleRatio = dial.ScaleRatio;
                }

                //align knot to needle
                if (k.AlignToNeedle)
                {
                    if (needles.Length > 0)
                    {
                        foreach(Needles n in needles)
                        {
                            if (n.tagName == k.NeedleTag)
                            {
                                posX = n.posPivot.x - (k.Texture.width * k.ScaleRatio) / 2;
                                posY = n.posPivot.y - (k.Texture.height * k.ScaleRatio) / 2;
                            }
                        }
                    }
                }

                //Speed knot
                GUI.DrawTexture(
                        new Rect(
                            posX,
                            posY,
                            k.Texture.width * (k.ScaleRatio),
                            k.Texture.height * (k.ScaleRatio)),
                        k.Texture);
            }
        }
    }

}
