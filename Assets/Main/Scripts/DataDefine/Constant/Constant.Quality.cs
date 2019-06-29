using UnityEngine;

namespace Genesis.GameClient
{
    public static partial class Constant
    {
        /// <summary>
        /// 道具、英雄品阶的常量。
        /// </summary>
        public static class Quality
        {
            public static readonly Color Undefined = new Color32(0, 0, 0, 255);
            public static readonly Color White = new Color32(183, 205, 212, 255);
            public static readonly Color Green = new Color32(130, 226, 87, 255);
            public static readonly Color Blue = new Color32(90, 182, 255, 255);
            public static readonly Color Purple = new Color32(219, 114, 249, 255);
            public static readonly Color Orange = new Color32(244, 215, 107, 255);
            public static readonly Color Red = new Color32(255, 99, 79, 255);

            public static readonly Color[] Colors = new Color[]
            {
                Undefined,
                White,
                Green,
                Blue,
                Purple,
                Orange,
                Red,
            };

            public static readonly Color QualityLevelUndefined = new Color32(0, 0, 0, 255);
            public static readonly Color QualityLevelWhite = new Color32(231, 241, 255, 255);
            public static readonly Color QualityLevelGreen = new Color32(213, 255, 188, 255);
            public static readonly Color QualityLevelBlue = new Color32(185, 244, 255, 255);
            public static readonly Color QualityLevelPurple = new Color32(232, 212, 249, 255);
            public static readonly Color QualityLevelOrange = new Color32(255, 242, 197, 255);
            public static readonly Color QualityLevelRed = new Color32(255, 99, 79, 255);

            public static readonly Color[] QualityLevelColors = new Color[]
            {
                QualityLevelUndefined,
                QualityLevelWhite,
                QualityLevelGreen,
                QualityLevelBlue,
                QualityLevelPurple,
                QualityLevelOrange,
                QualityLevelRed,
            };

            public static readonly string[] ItemBorderSpriteNames = new string[]
            {
                "", // Quality starts at one. See QualityType.
                "border_item_silvery",
                "border_item_green",
                "border_item_blue",
                "border_item_purple",
                "border_item_orange",
                "border_item_red",
            };

            public static readonly string[] HeroBorderSpriteNames = new string[]
            {
                "", // Quality starts at one. See QualityType.
                "border_hero_silvery",
                "border_hero_green",
                "border_hero_blue",
                "border_hero_purple",
                "border_hero_orange",
                "border_hero_red",
            };

            public static readonly string[] HeroBorderCornerSpriteNames = new string[]
            {
                "", // Quality starts at one. See QualityType.
                "border_quality_silvery",
                "border_quality_green",
                "border_quality_blue",
                "border_quality_purple",
                "border_quality_orange",
                "border_quality_red",
            };

            public static readonly string[] HeroSquareBorderSpriteNames = new string[]
            {
                "", // Quality starts at one. See QualityType.
                "frame_card_quality_ordinary",
                "frame_card_quality_green",
                "frame_card_quality_blue",
                "frame_card_quality_purple",
                "frame_card_quality_gold",
                "frame_card_quality_ordinary",
            };

            public static readonly string[] HeroQualityLevelSpriteNames = new string[]
            {
                "",
                "icon_quality_silvery",
                "icon_quality_green",
                "icon_quality_blue",
                "icon_quality_purple",
                "icon_quality_orange",
            };

            public static readonly string[] HeroQualityLevelBorderSpriteNames = new string[]
            {
                "", // Quality starts at one. See QualityType.
                "border_portrait_silvery",
                "border_portrait_green",
                "border_portrait_blue",
                "border_portrait_purple",
                "border_portrait_orange",
            };
        }
    }
}
