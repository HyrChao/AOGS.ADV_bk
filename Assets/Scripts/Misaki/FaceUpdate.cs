//UnityChan
//2017/4/1
//Modified By Chao
using UnityEngine;
using System.Collections;
	public class FaceUpdate : MonoBehaviour
	{
		public AnimationClip[] animations;
		Animator anim;
        Player player;
		public float delayWeight;
		public bool isKeepFace = false;
        public bool isGUI = true;

        //攻击时的表情持续时间
        private float atkEmoTime=2f;
        private float firEmoTime=2f;
        public void atkEmo()
        {
            ChangeFace("angry");
        }
        public void firEmo()
        {
            ChangeFace("scold"); 
        }
        public void norEmo()
        {
            ChangeFace("default");
        }


        void Start ()
		{
			anim = GetComponent<Animator> ();
            player = gameObject.GetComponentInParent<Player>();
		}
		float current = 0;

		void Update ()
		{
            if (Input.GetButtonDown("Debug")) {
				current = 1;
			} else if (!isKeepFace) {
				current = Mathf.Lerp (current, 0, delayWeight);
			}
			anim.SetLayerWeight (1, current);
		}
	 

		//アニメーションEvents側につける表情切り替え用イベントコール
        //动画事件切换时用的事件代码
		public void OnCallChangeFace (string str)
		{   
			int ichecked = 0;
			foreach (var animation in animations) {
				if (str == animation.name) {
					ChangeFace (str);
					break;
				} else if (ichecked <= animations.Length) {
					ichecked++;
				} else {
					//str指定错误时回归初始值
					str = "default@unitychan";
					ChangeFace (str);
				}
			} 
		}

		void ChangeFace (string str)
		{
			//isKeepFace = true;
			current = 1;
			anim.CrossFade (str, 0);
		}
        
        //GUI
        void OnGUI()
        {
            if (isGUI)
            {
                GUILayout.Box("Face Update", GUILayout.Width(170), GUILayout.Height(25 * (animations.Length + 2)));
                Rect screenRect = new Rect(10, 25, 150, 25 * (animations.Length + 1));
                GUILayout.BeginArea(screenRect);
                foreach (var animation in animations)
                {
                    if (GUILayout.RepeatButton(animation.name))
                    {
                        anim.CrossFade(animation.name, 0);
                    }
                }
                isKeepFace = GUILayout.Toggle(isKeepFace, " Keep Face");
                GUILayout.EndArea();
            }
        }
    }
