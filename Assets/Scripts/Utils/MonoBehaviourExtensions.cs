using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
    public static class MonoBehaviourExtensions
    {
        public static void CallWithDelay(this MonoBehaviour mono, Action method, float delay = 0f)
        {
            mono.StartCoroutine(CallWithDelayCoroutine(method, delay));
        }

        private static IEnumerator CallWithDelayCoroutine(Action method, float delay)
        {
            yield return new WaitForSeconds(delay);
            method();
        }

        public static void CallActionNextFrame(this MonoBehaviour mono, Action method)
        {
            mono.StartCoroutine(CallNextFrame(method));
        }

        private static IEnumerator CallNextFrame(Action method)
        {
            yield return new WaitForEndOfFrame();

            method();
        }
    }
}