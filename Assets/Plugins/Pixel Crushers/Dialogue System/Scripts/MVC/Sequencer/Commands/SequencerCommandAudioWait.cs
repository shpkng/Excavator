// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    /// <summary>
    /// Implements sequencer command: AudioWait(audioClip[, subject[, audioClips...]])
    /// </summary>
    [AddComponentMenu("")] // Hide from menu.
    public class SequencerCommandAudioWait : SequencerCommand
    {

        protected float stopTime = 0;
        protected AudioSource audioSource = null;
        protected int nextClipIndex = 2;
        protected string audioClipName;
        protected AudioClip currentClip = null;
        protected AudioClip originalClip = null;
        protected bool restoreOriginalClip = false; // Don't restore; could stop next entry's AudioWait that runs same frame.
        protected bool playedAudio = false;
        protected bool isLoadingAudio = false;

        public virtual IEnumerator Start()
        {
            audioClipName = GetParameter(0);
            Transform subject = GetSubject(1);
            nextClipIndex = 2;
            audioSource = GetAudioSource(subject);
            //--- Logged in TryAudioClip: if (DialogueDebug.logInfo) Debug.Log(string.Format("{0}: Sequencer: AudioWait({1}) on {2}", new System.Object[] { DialogueDebug.Prefix, GetParameters(), audioSource }), audioSource);
            if (audioSource == null)
            {
                if (DialogueDebug.logWarnings)
                {
                    if (subject == null)
                    {
                        Debug.LogWarning(string.Format("{0}: Sequencer: AudioWait({1}) command: can't find or add AudioSource. Subject is null.", new System.Object[] { DialogueDebug.Prefix, GetParameters() }));
                    }
                    else
                    {
                        Debug.LogWarning(string.Format("{0}: Sequencer: AudioWait({1}) command: can't find or add AudioSource to {2}.", new System.Object[] { DialogueDebug.Prefix, GetParameters(), subject.name }), subject);
                    }
                }
                Stop();
            }
            else
            {
                originalClip = audioSource.clip;
                stopTime = DialogueTime.time + 1; // Give time for yield return null.
                yield return null;
                originalClip = audioSource.clip;
                TryAudioClip(audioClipName);
            }
        }

        protected virtual AudioSource GetAudioSource(Transform subject)
        {
            return SequencerTools.GetAudioSource(subject);
        }

        protected virtual void TryAudioClip(string audioClipName)
        {
            try
            {
                if (string.IsNullOrEmpty(audioClipName))
                {
                    if (DialogueDebug.logWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: AudioWait() command: Audio clip name is blank.", new System.Object[] { DialogueDebug.Prefix }));
                    stopTime = 0;
                    if (nextClipIndex >= parameters.Length)
                    {
                        Stop();
                    }
                }
                else
                {
                    this.audioClipName = audioClipName;
                    isLoadingAudio = true;
                    DialogueManager.LoadAsset(audioClipName, typeof(AudioClip),
                        (asset) =>
                        {
                            isLoadingAudio = false;
                            var audioClip = asset as AudioClip;
                            if (audioClip == null)
                            {
                                if (DialogueDebug.logWarnings && Sequencer.reportMissingAudioFiles) Debug.LogWarning(string.Format("{0}: Sequencer: AudioWait() command: Clip '{1}' wasn't found.", new System.Object[] { DialogueDebug.Prefix, audioClipName }));
                                stopTime = 0;
                                if (nextClipIndex >= parameters.Length)
                                {
                                    Stop();
                                }
                            }
                            else
                            {
                                if (IsAudioMuted())
                                {
                                    if (DialogueDebug.logInfo) Debug.Log(string.Format("{0}: Sequencer: AudioWait(): waiting but not playing '{1}'; audio is muted.", new System.Object[] { DialogueDebug.Prefix, audioClipName }));
                                }
                                else if (audioSource != null) // Check in case AudioSource was destroyed while loading Addressable.
                                {
                                    if (DialogueDebug.logInfo) Debug.Log(string.Format("{0}: Sequencer: AudioWait(): playing '{1}' on {2}.", new System.Object[] { DialogueDebug.Prefix, audioClipName, audioSource }), audioSource);
                                    currentClip = audioClip;
                                    audioSource.clip = audioClip;
                                    audioSource.Play();
                                }
                                playedAudio = true;
                                stopTime = DialogueTime.time + GetAudioClipLength(audioSource, audioClip);
                            }
                        });
                }

            }
            catch (System.Exception)
            {
                stopTime = 0;
            }
        }

        public static float GetAudioClipLength(AudioSource audioSource, AudioClip audioClip)
        {
            if (audioClip == null) return 0;
            if (audioSource == null) return audioClip.length;
            var pitchAbs = Mathf.Abs(audioSource.pitch);
            if (Time.timeScale > 0)
            {
                if (pitchAbs == 1 || Mathf.Approximately(0, pitchAbs))
                    return audioClip.length / Time.timeScale;
                else
                    return (audioClip.length / Time.timeScale) / pitchAbs;
            }
            else
            {
                if (pitchAbs == 1 || Mathf.Approximately(0, pitchAbs))
                    return audioClip.length;
                else
                    return audioClip.length / pitchAbs;
            }
        }

        public virtual void Update()
        {
            if (DialogueTime.time >= stopTime)
            {
                if (currentClip != null)
                {
                    DialogueManager.UnloadAsset(currentClip);
                }
                currentClip = null;
                if (!isLoadingAudio)
                {
                    if (nextClipIndex < parameters.Length)
                    {
                        TryAudioClip(GetParameter(nextClipIndex));
                        nextClipIndex++;
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
        }

        public virtual void OnDialogueSystemPause()
        {
            if (audioSource == null) return;
            audioSource.Pause();
        }

        public virtual void OnDialogueSystemUnpause()
        {
            if (audioSource == null) return;
            audioSource.Play();
        }

        public virtual void OnDestroy()
        {
            if (audioSource != null)
            {
                if (audioSource.isPlaying && 
                    (audioSource.clip == currentClip) &&
                    (audioSource.clip != null))
                {
                    audioSource.Stop();
                }
                if (restoreOriginalClip) audioSource.clip = originalClip;
                DialogueManager.UnloadAsset(currentClip);
            }
        }

    }

}
