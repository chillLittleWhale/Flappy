using System;
using UnityEngine;

namespace AjaxNguyen.Core.Audio
{
    [Serializable]
    public struct SoundGroup
    {
        public string groupId;
        public AudioClip[] clips;
    }

    public class SoundLibrary : MonoBehaviour
    {
        [SerializeField] SoundGroup[] soundGroups;

        public AudioClip[] GetAudioClips(string soundGroupId)
        {
            foreach (var group in soundGroups)
            {
                if (string.Equals(soundGroupId, group.groupId))
                {
                    return group.clips;
                }
            }
            return null;
        }

        public AudioClip GetAudioClip(string clipName)
        {
            foreach (var group in soundGroups)
            {
                foreach (var clip in group.clips)
                {
                    if (string.Equals(clipName, clip.name))
                    {
                        return clip;
                    }
                }
            }
            return null;
        }
    }
}
