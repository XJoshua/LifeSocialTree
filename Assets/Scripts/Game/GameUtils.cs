using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GameUtils
    {
        // number scroll
        // number scroll
        public static void TextNumberAnim(long start, long end, TextMeshProUGUI text)
        {
            var time = Math.Min((end - start) / 20f, 0.5f);
            time = time < 0 ? 0 : time;

            var se = DOTween.Sequence();

            se.Append(DOTween.To(delegate(float value)
            {
                var temp = Mathf.FloorToInt(value);
                text.text = temp.ToString();
            }, start, end, time));
        }
    }
}