using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Effects/Gradient")]
#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9
    public class Gradient : BaseMeshEffect
#else
    public class Gradient : BaseMeshEffect
#endif
	{
        public enum Type
        {
            Vertical,
            Horizontal
        }
        [SerializeField]
        public Type GradientType = Type.Vertical;

        [SerializeField]
        [Range(-1.5f, 1.5f)]
        public float Offset = 0f;

        [SerializeField]
        private Color32 StartColor = Color.white;
        [SerializeField]
        private Color32 EndColor = Color.black;

        public override void ModifyMesh(VertexHelper helper)
        {
            if (!IsActive() || helper.currentVertCount == 0)
                return;

            List<UIVertex> _vertexList = new List<UIVertex>();
            helper.GetUIVertexStream(_vertexList);

            int nCount = _vertexList.Count;
            switch (GradientType) {
                case Type.Vertical: {
                        float fBottomY = _vertexList[0].position.y;
                        float fTopY = _vertexList[0].position.y;
                        float fYPos = 0f;

                        for (int i = nCount - 1; i >= 1; --i) {
                            fYPos = _vertexList[i].position.y;
                            if (fYPos > fTopY)
                                fTopY = fYPos;
                            else if (fYPos < fBottomY)
                                fBottomY = fYPos;
                        }

                        float fUIElementHeight = 1f / (fTopY - fBottomY);
                        UIVertex v = new UIVertex();

                        for (int i = 0; i < helper.currentVertCount; i++) {
                            helper.PopulateUIVertex(ref v, i);
                            v.color = Color32.Lerp(EndColor, StartColor, (v.position.y - fBottomY) * fUIElementHeight - Offset);
                            helper.SetUIVertex(v, i);
                        }
                    }
                    break;
                case Type.Horizontal: {
                        float fLeftX = _vertexList[0].position.x;
                        float fRightX = _vertexList[0].position.x;
                        float fXPos = 0f;

                        for (int i = nCount - 1; i >= 1; --i) {
                            fXPos = _vertexList[i].position.x;
                            if (fXPos > fRightX)
                                fRightX = fXPos;
                            else if (fXPos < fLeftX)
                                fLeftX = fXPos;
                        }

                        float fUIElementWidth = 1f / (fRightX - fLeftX);
                        UIVertex v = new UIVertex();

                        for (int i = 0; i < helper.currentVertCount; i++) {
                            helper.PopulateUIVertex(ref v, i);
                            v.color = Color32.Lerp(EndColor, StartColor, (v.position.x - fLeftX) * fUIElementWidth - Offset);
                            helper.SetUIVertex(v, i);
                        }

                    }
                    break;
                default:
                    break;
            }
        }
    }
}