using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class CharacterSkin : MonoBehaviour
{
    [Header("皮肤")]
    SkeletonMecanim skeletonAnimation;
    //public GameObject Animation_Spine;//Animation_Spine的动画机
    Skin blendSkin = new Skin("BlendedSkin");// 创建一个新的混合皮肤





    // Start is called before the first frame update
    void Start()
    {
        //换皮肤
        skeletonAnimation = GetComponent<SkeletonMecanim>();

        //初始皮肤
        ShowCurrentAll();
    }
    public void ShowCurrentAll()
    {
        //初始设置为混合皮肤
        //ShowCurrentBody();
        //ShowCurrentHead();
        //ShowCurrentLegs();

        blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color1"));
        blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color1"));
        blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color1"));
        blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color10"));//叶语嫣发饰

        skeletonAnimation.Skeleton.SetSkin(blendSkin);
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
    }

    //public void ShowCurrentBody() { SetClothes(PlayerPrefs.GetInt("current_Body")); }
    //public void ShowCurrentHead() { SetClothes(PlayerPrefs.GetInt("current_Head") + 100); }
    //public void ShowCurrentLegs() { SetClothes(-PlayerPrefs.GetInt("current_Legs")); }

    //public void SetClothes(int Color)//是否需要文字描述
    //{
    //
    //    switch (Color)
    //    {
    //
    //        case 1:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color1"));
    //            PlayerPrefs.SetInt("current_Body", 1);
    //            Icon_Body.sprite = Body_1;
    //            BodyBulletProof = PlayerPrefs.GetInt("Body_Color01") * 100;
    //            break;
    //        case 2:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color2"));
    //            PlayerPrefs.SetInt("current_Body", 2);
    //            Icon_Body.sprite = Body_2;
    //            BodyBulletProof = PlayerPrefs.GetInt("Body_Color02") * 100;
    //            break;
    //        case 3:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color3"));
    //            PlayerPrefs.SetInt("current_Body", 3);
    //            Icon_Body.sprite = Body_3;
    //            BodyBulletProof = PlayerPrefs.GetInt("Body_Color03") * 100;
    //            break;
    //        case 4:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color4"));
    //            PlayerPrefs.SetInt("current_Body", 4);
    //            Icon_Body.sprite = Body_4;
    //            BodyBulletProof = PlayerPrefs.GetInt("Body_Color04") * 100;
    //            break;
    //        case 5:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color5"));
    //            PlayerPrefs.SetInt("current_Body", 5);
    //            Icon_Body.sprite = Body_5;
    //            BodyBulletProof = PlayerPrefs.GetInt("Body_Color05") * 100;
    //            break;
    //        case 6:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color6"));
    //            PlayerPrefs.SetInt("current_Body", 6);
    //            Icon_Body.sprite = Body_6;
    //            BodyBulletProof = PlayerPrefs.GetInt("Body_Color06") * 100;
    //            break;
    //        case 7:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color7"));
    //            PlayerPrefs.SetInt("current_Body", 7);
    //            Icon_Body.sprite = Body_7;
    //            BodyBulletProof = PlayerPrefs.GetInt("Body_Color07") * 100;
    //            break;
    //        case 8:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color8"));
    //            PlayerPrefs.SetInt("current_Body", 8);
    //            Icon_Body.sprite = Body_8;
    //            BodyBulletProof = PlayerPrefs.GetInt("Body_Color08") * 100;
    //            break;
    //        case 9:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color9"));
    //            PlayerPrefs.SetInt("current_Body", 9);
    //            Icon_Body.sprite = Body_9;
    //            BodyBulletProof = PlayerPrefs.GetInt("Body_Color09") * 100;
    //            break;
    //        case 10:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color10"));
    //            PlayerPrefs.SetInt("current_Body", 10);
    //            Icon_Body.sprite = Body_10;
    //            BodyBulletProof = PlayerPrefs.GetInt("Body_Color10") * 100;
    //            break;
    //        case 11:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color11"));
    //            break;
    //        case 12:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Body/YYY_Body_color12"));
    //            PlayerPrefs.SetInt("current_Body", 12);
    //            Icon_Body.sprite = Body_12;
    //            BodyBulletProof = 0;
    //            break;
    //
    //
    //
    //
    //
    //        case -1:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color1"));
    //            PlayerPrefs.SetInt("current_Legs", 1);
    //            Icon_Legs.sprite = Legs_1;
    //            LegsBulletProof = PlayerPrefs.GetInt("Legs_Color01") * 100;
    //            break;
    //        case -2:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color2"));
    //            PlayerPrefs.SetInt("current_Legs", 2);
    //            Icon_Legs.sprite = Legs_2;
    //            LegsBulletProof = PlayerPrefs.GetInt("Legs_Color02") * 100;
    //            break;
    //        case -3:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color3"));
    //            PlayerPrefs.SetInt("current_Legs", 3);
    //            Icon_Legs.sprite = Legs_3;
    //            LegsBulletProof = PlayerPrefs.GetInt("Legs_Color03") * 100;
    //            break;
    //        case -4:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color4"));
    //            PlayerPrefs.SetInt("current_Legs", 4);
    //            Icon_Legs.sprite = Legs_4;
    //            LegsBulletProof = PlayerPrefs.GetInt("Legs_Color04") * 100;
    //            break;
    //        case -5:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color5"));
    //            PlayerPrefs.SetInt("current_Legs", 5);
    //            Icon_Legs.sprite = Legs_5;
    //            LegsBulletProof = PlayerPrefs.GetInt("Legs_Color05") * 100;
    //            break;
    //        case -6:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color6"));
    //            PlayerPrefs.SetInt("current_Legs", 6);
    //            Icon_Legs.sprite = Legs_6;
    //            LegsBulletProof = PlayerPrefs.GetInt("Legs_Color06") * 100;
    //            break;
    //        case -7:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color7"));
    //            PlayerPrefs.SetInt("current_Legs", 7);
    //            Icon_Legs.sprite = Legs_7;
    //            LegsBulletProof = PlayerPrefs.GetInt("Legs_Color07") * 100;
    //            break;
    //        case -8:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color8"));
    //            PlayerPrefs.SetInt("current_Legs", 8);
    //            Icon_Legs.sprite = Legs_8;
    //            LegsBulletProof = PlayerPrefs.GetInt("Legs_Color08") * 100;
    //            break;
    //        case -9:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color9"));
    //            PlayerPrefs.SetInt("current_Legs", 9);
    //            Icon_Legs.sprite = Legs_9;
    //            LegsBulletProof = PlayerPrefs.GetInt("Legs_Color09") * 100;
    //            break;
    //        case -10:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color10"));
    //            PlayerPrefs.SetInt("current_Legs", 10);
    //            Icon_Legs.sprite = Legs_10;
    //            LegsBulletProof = PlayerPrefs.GetInt("Legs_Color10") * 100;
    //            break;
    //        case -11:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color11"));
    //            break;
    //        case -12:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Legs/YYY_Legs_color12"));
    //            PlayerPrefs.SetInt("current_Legs", 12);
    //            Icon_Legs.sprite = Legs_12;
    //            LegsBulletProof = 0;
    //            break;
    //
    //
    //
    //
    //        case 101:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color1"));
    //            PlayerPrefs.SetInt("current_Head", 1);
    //            Icon_Head.sprite = Head_1;
    //
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color10"));//叶语嫣发饰
    //
    //            break;
    //        case 102:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color2"));
    //            PlayerPrefs.SetInt("current_Head", 2);
    //            Icon_Head.sprite = Head_2;
    //
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color1"));//无
    //
    //            break;
    //        case 103:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color3"));
    //            PlayerPrefs.SetInt("current_Head", 3);
    //            Icon_Head.sprite = Head_3;
    //
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color1"));//无
    //
    //            break;
    //        case 104:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color4"));
    //            PlayerPrefs.SetInt("current_Head", 4);
    //            Icon_Head.sprite = Head_4;
    //
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color1"));//无
    //
    //            break;
    //        case 105:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color5"));
    //            PlayerPrefs.SetInt("current_Head", 5);
    //            Icon_Head.sprite = Head_5;
    //
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color1"));//无
    //
    //            break;
    //        case 106:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color6"));
    //            PlayerPrefs.SetInt("current_Head", 6);
    //            Icon_Head.sprite = Head_6;
    //
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color12"));//恶魔红色尖角
    //
    //            break;
    //        case 107:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color7"));
    //            PlayerPrefs.SetInt("current_Head", 7);
    //            Icon_Head.sprite = Head_7;
    //
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color1"));//无
    //            break;
    //        case 108:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color8"));
    //            PlayerPrefs.SetInt("current_Head", 8);
    //            Icon_Head.sprite = Head_8;
    //
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color1"));//无
    //
    //            break;
    //        case 109:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color9"));
    //            PlayerPrefs.SetInt("current_Head", 9);
    //            Icon_Head.sprite = Head_9;
    //
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color9"));//红色头箍
    //
    //            break;
    //        case 110:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color10"));
    //            PlayerPrefs.SetInt("current_Head", 10);
    //            Icon_Head.sprite = Head_10;
    //
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color1"));//无
    //
    //            break;
    //        case 111:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color11"));
    //            PlayerPrefs.SetInt("current_Head", 11);
    //            Icon_Head.sprite = Head_11;
    //
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color4"));//兔女郎发饰
    //
    //            break;
    //        case 112:
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Head/YYY_Head_color12"));
    //            PlayerPrefs.SetInt("current_Head", 12);
    //            Icon_Head.sprite = Head_12;
    //
    //            blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin("YYY/Hat/YYY_Hat_color12"));//恶魔红色尖角
    //
    //            break;
    //
    //
    //    }
    //
    //    skeletonAnimation.Skeleton.SetSkin(blendSkin);
    //    skeletonAnimation.Skeleton.SetSlotsToSetupPose();
    //
    //
    //    
    //
    //
    //
    //    
    //
    //    
    //
    //    
    //
    //}//这里是换装＋更新描述的地方
}
