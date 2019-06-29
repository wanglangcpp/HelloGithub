namespace Genesis.GameClient.Editor
{
    public class ShaderGUIDInfo
    {
        public class ShaderGUID
        {
            public string Name;
            public int FileID;
            public string GUID;
            public int Type;

            public ShaderGUID(string name, int fileID, string guid, int type)
            {
                Name = name;
                FileID = fileID;
                GUID = guid;
                Type = type;
            }
        }

        public static ShaderGUID[] ShaderGUIDs = new ShaderGUID[]
        {
            new ShaderGUID("GUI/Text Shader", 10101, "0000000000000000e000000000000000", 0),
            new ShaderGUID("Sprites/Default", 10753, "0000000000000000e000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/Bumped Diffuse", 31, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/Bumped Specular", 33, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/Diffuse", 30, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/Specular", 32, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/Parallax Diffuse", 35, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/Parallax Specular", 36, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/VertexLit", 34, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/Cutout/Bumped Diffuse", 52, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/Cutout/Bumped Specular", 54, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/Cutout/Diffuse", 51, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/Cutout/Specular", 53, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/Cutout/Soft Edge Unlit", 10512, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Transparent/Cutout/VertexLit", 50, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Decal", 100, "0000000000000000f000000000000000", 0),
            new ShaderGUID("FX/Flare", 101, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Self Illumin/Bumped Diffuse", 11, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Self Illumin/Bumped Specular", 13, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Self Illumin/Diffuse", 10, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Self Illumin/Specular", 12, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Self Illumin/Parallax Diffuse", 15, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Self Illumin/Parallax Specular", 16, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Self Illumin/VertexLit", 14, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Lightmapped/Bumped Diffuse", 42, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Lightmapped/Bumped Specular", 44, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Lightmapped/Diffuse", 41, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Lightmapped/Specular", 43, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Lightmapped/VertexLit", 40, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Bumped Diffuse", 2, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Bumped Specular", 4, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Diffuse", 7, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Diffuse Detail", 5, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Diffuse Fast", 1, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Specular", 3, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Parallax Diffuse", 8, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Parallax Specular", 9, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/VertexLit", 6, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Particles/Additive", 200, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Particles/~Additive Multiply", 201, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Particles/Additive (Soft)", 202, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Particles/Alpha Blended", 203, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Particles/Multiply", 205, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Particles/Multiply (Double)", 206, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Particles/Alpha Blended Premultiply", 207, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Particles/VertexLit Blended", 208, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Reflective/Bumped Diffuse", 21, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Reflective/Bumped Unlit", 25, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Reflective/Bumped Specular", 23, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Reflective/Bumped VertexLit", 26, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Reflective/Diffuse", 20, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Reflective/Specular", 22, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Reflective/Parallax Diffuse", 27, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Reflective/Parallax Specular", 28, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Legacy Shaders/Reflective/VertexLit", 24, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Skybox/Cubemap", 103, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Skybox/Procedural", 106, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Skybox/6 Sided", 104, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Sprites/Diffuse", 10800, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Standard", 46, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Standard (Specular setup)", 45, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Mobile/Bumped Diffuse", 10704, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Mobile/Bumped Specular (1 Directional Light)", 10706, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Mobile/Bumped Specular", 10705, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Mobile/Diffuse", 10703, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Mobile/Unlit (Supports Lightmap)", 10708, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Mobile/Particles/Additive", 10720, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Mobile/Particles/VertexLit Blended", 10722, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Mobile/Particles/Alpha Blended", 10721, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Mobile/Particles/Multiply", 10723, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Mobile/Skybox", 10700, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Mobile/VertexLit (Only Directional Lights)", 10707, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Mobile/VertexLit", 10701, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Nature/SpeedTree", 14000, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Nature/SpeedTree Billboard", 14001, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Nature/Tree Soft Occlusion Bark", 10509, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Nature/Tree Soft Occlusion Leaves", 10511, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Nature/Tree Creator Bark", 10600, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Nature/Tree Creator Leaves", 10601, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Nature/Tree Creator Leaves Fast", 10606, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Nature/Terrain/Diffuse", 10505, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Nature/Terrain/Specular", 10620, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Nature/Terrain/Standard", 10623, "0000000000000000f000000000000000", 0),
            new ShaderGUID("UI/Default", 10770, "0000000000000000f000000000000000", 0),
            new ShaderGUID("UI/Default Font", 10782, "0000000000000000f000000000000000", 0),
            new ShaderGUID("UI/Lit/Bumped", 10765, "0000000000000000f000000000000000", 0),
            new ShaderGUID("UI/Lit/Detail", 10766, "0000000000000000f000000000000000", 0),
            new ShaderGUID("UI/Lit/Refraction", 10767, "0000000000000000f000000000000000", 0),
            new ShaderGUID("UI/Lit/Refraction Detail", 10768, "0000000000000000f000000000000000", 0),
            new ShaderGUID("UI/Lit/Transparent", 10764, "0000000000000000f000000000000000", 0),
            new ShaderGUID("UI/Unlit/Detail", 10761, "0000000000000000f000000000000000", 0),
            new ShaderGUID("UI/Unlit/Text", 10762, "0000000000000000f000000000000000", 0),
            new ShaderGUID("UI/Unlit/Text Detail", 10763, "0000000000000000f000000000000000", 0),
            new ShaderGUID("UI/Unlit/Transparent", 10760, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Unlit/Transparent", 10750, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Unlit/Transparent Cutout", 10751, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Unlit/Color", 10755, "0000000000000000f000000000000000", 0),
            new ShaderGUID("Unlit/Texture", 10752, "0000000000000000f000000000000000", 0),
        };
    }
}
