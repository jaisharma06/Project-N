using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectN.Characters.UI
{
    public class ProgressBarHandler : MonoBehaviour
    {
        public enum Type
        {
            Progress,
            Instant,
        }

        [SerializeField] private Graphic m_TargetGraphic;
        [SerializeField] private Type m_Type = Type.Progress;
        private bool ascending = false;

        [SerializeField] private Text progressLabel;
        [SerializeField] private float progressSpeed = 1f;
        [SerializeField] private string appendBefore = "";
        [SerializeField] private string appendAfter = "%";

        [SerializeField] private Color[] colors;
        [SerializeField] private float colorLerp = 4f;

        private float _value;
        private float _targetValue { get; set; }
        private Coroutine _setValueCoroutine;

        protected void Start()
        {
            Init();
        }

        protected void Update()
        {
            if (this.m_TargetGraphic == null || (this.m_TargetGraphic is Image) == false || this.m_Type != Type.Progress)
                return;

            // Change the bar color based on the percentage
            if (this.colors.Length > 1) {
                Image image = this.m_TargetGraphic as Image;

                if (image != null) {
                    // As default
                    int colorIndex = 0;

                    // If we're over or at 100 percent
                    if (image.fillAmount >= 1f) {
                        colorIndex = this.colors.Length - 1;
                    } else if (image.fillAmount > 0f) {
                        colorIndex = Mathf.FloorToInt(this.colors.Length * image.fillAmount);
                    }

                    image.canvasRenderer.SetColor(Color.Lerp(image.canvasRenderer.GetColor(), this.colors[colorIndex], Time.deltaTime * this.colorLerp));
                }
            }
        }

        public void Init()
        {
            if (m_TargetGraphic == null)
                return;

            if (m_TargetGraphic is Image && this.m_Type == Type.Progress) {
                Image image = (this.m_TargetGraphic as Image);

                if (image.type != Image.Type.Filled)
                    image.type = Image.Type.Filled;

                image.fillAmount = 1f;
                _value = 1f;
            }
        }

        private void UpdateProgressLabel()
        {
            if (this.m_TargetGraphic == null || (this.m_TargetGraphic is Image) == false || this.m_Type != Type.Progress)
                return;

            Image image = this.m_TargetGraphic as Image;

            if (image != null && this.progressLabel != null)
                this.progressLabel.text = this.appendBefore + (image.fillAmount * 100).ToString("0") + this.appendAfter;
        }

        public void SetValue(float targetValue)
        {
            if(_setValueCoroutine != null) {
                StopCoroutine(_setValueCoroutine);
            }
            _setValueCoroutine = StartCoroutine(IE_SetValue(targetValue));
        }

        private IEnumerator IE_SetValue(float targetValue)
        {
            if (!m_TargetGraphic) {
                yield break;
            }

            Image image = this.m_TargetGraphic as Image;
            _targetValue = targetValue;
            if(m_Type == Type.Progress) {
                while (_value > _targetValue) {
                    _value = Mathf.MoveTowards(image.fillAmount, _targetValue, progressSpeed * Time.deltaTime);
                    image.fillAmount = _value;
                    yield return new WaitForEndOfFrame();
                }
            } else {
                _value = targetValue;
                image.fillAmount = _value;
            }

            UpdateProgressLabel();
            yield return null;
        }
    }
}
