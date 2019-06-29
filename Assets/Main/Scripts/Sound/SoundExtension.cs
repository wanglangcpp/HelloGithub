using GameFramework;
using GameFramework.DataTable;
using GameFramework.Sound;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public static class SoundExtension
    {
        public static int PlayMusic(this SoundComponent soundComponent, int musicId, object userData = null)
        {
            IDataTable<DRMusic> dtMusic = GameEntry.DataTable.GetDataTable<DRMusic>();
            DRMusic dataRow = dtMusic.GetDataRow(musicId);
            if (dataRow == null)
            {
                Log.Warning("Can not load music '{0}' from data table.", musicId.ToString());
                return -1;
            }

            PlaySoundParams playSoundParams = new PlaySoundParams
            {
                Priority = 64,
                Loop = true,
                VolumeInSoundGroup = 1f,
                SpatialBlend = 0f,
            };
            return soundComponent.PlaySound(AssetUtility.GetMusicAsset(dataRow.ResourceName), "Music", playSoundParams, null, userData);
        }

        public static int PlaySound(this SoundComponent soundComponent, int soundId, Entity bindingEntity = null, object userData = null)
        {
            IDataTable<DRSound> dtSound = GameEntry.DataTable.GetDataTable<DRSound>();
            DRSound dataRow = dtSound.GetDataRow(soundId);
            if (dataRow == null)
            {
                Log.Warning("Can not load sound '{0}' from data table.", soundId.ToString());
                return -1;
            }

            PlaySoundParams playSoundParams = new PlaySoundParams
            {
                Priority = dataRow.Priority,
                Loop = dataRow.Loop,
                VolumeInSoundGroup = dataRow.Volume,
                SpatialBlend = dataRow.SpatialBlend,
            };
            return soundComponent.PlaySound(AssetUtility.GetSoundAsset(dataRow.ResourceName), "Sound", playSoundParams, bindingEntity != null ? bindingEntity.Entity : null, userData);
        }

        public static bool MusicIsMuted(this SoundComponent soundComponent)
        {
            ISoundGroup musicGroup = soundComponent.GetSoundGroup("Music");
            if (musicGroup == null)
            {
                Log.Warning("Sound group 'Music' is invalid.");
                return true;
            }

            return musicGroup.Mute;
        }

        public static void MuteMusic(this SoundComponent soundComponent, bool mute)
        {
            GameEntry.Setting.SetBool(Constant.Setting.MusicIsMuted, mute);
            GameEntry.Setting.Save();

            ISoundGroup musicGroup = soundComponent.GetSoundGroup("Music");
            if (musicGroup == null)
            {
                Log.Warning("Sound group 'Music' is invalid.");
                return;
            }

            musicGroup.Mute = mute;
        }

        public static float GetMusicVolume(this SoundComponent soundComponent)
        {
            ISoundGroup musicGroup = soundComponent.GetSoundGroup("Music");
            if (musicGroup == null)
            {
                Log.Warning("Sound group 'Music' is invalid.");
                return 0f;
            }

            return musicGroup.Volume;
        }

        public static void SetMusicVolume(this SoundComponent soundComponent, float volume)
        {
            GameEntry.Setting.SetFloat(Constant.Setting.MusicVolume, volume);
            GameEntry.Setting.Save();

            ISoundGroup musicGroup = soundComponent.GetSoundGroup("Music");
            if (musicGroup == null)
            {
                Log.Warning("Sound group 'Music' is invalid.");
                return;
            }

            musicGroup.Volume = volume;
        }

        public static bool SoundIsMuted(this SoundComponent soundComponent)
        {
            ISoundGroup soundGroup = soundComponent.GetSoundGroup("Sound");
            if (soundGroup == null)
            {
                Log.Warning("Sound group 'Sound' is invalid.");
                return true;
            }

            return soundGroup.Mute;
        }

        public static void MuteSound(this SoundComponent soundComponent, bool mute)
        {
            GameEntry.Setting.SetBool(Constant.Setting.SoundIsMuted, mute);
            GameEntry.Setting.Save();

            NGUITools.soundVolume = mute ? 0f : soundComponent.GetSoundVolume();

            ISoundGroup soundGroup = soundComponent.GetSoundGroup("Sound");
            if (soundGroup == null)
            {
                Log.Warning("Sound group 'Sound' is invalid.");
                return;
            }

            soundGroup.Mute = mute;
        }

        public static float GetSoundVolume(this SoundComponent soundComponent)
        {
            ISoundGroup soundGroup = soundComponent.GetSoundGroup("Sound");
            if (soundGroup == null)
            {
                Log.Warning("Sound group 'Sound' is invalid.");
                return 0f;
            }

            return soundGroup.Volume;
        }

        public static void SetSoundVolume(this SoundComponent soundComponent, float volume)
        {
            GameEntry.Setting.SetFloat(Constant.Setting.SoundVolume, volume);
            GameEntry.Setting.Save();

            NGUITools.soundVolume = soundComponent.SoundIsMuted() ? 0f : volume;

            ISoundGroup soundGroup = soundComponent.GetSoundGroup("Sound");
            if (soundGroup == null)
            {
                Log.Warning("Sound group 'Sound' is invalid.");
                return;
            }

            soundGroup.Volume = volume;
        }
    }
}
