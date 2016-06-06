using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO.Ports;

[RequireComponent(typeof(AudioSource))]

public class playvideo : MonoBehaviour {
    public SerialPort sp = new SerialPort("COM4", 9600);
    
    [Tooltip("GUI-Text to display debug messages.")]
    public GUIText videoInfo;

    public MovieTexture movieC1;
    public MovieTexture movieA1;
    public MovieTexture movieB1;
    public MovieTexture movieC2;
    public MovieTexture movieA2;
    public MovieTexture movieB2;
    public MovieTexture movieC3;
    public MovieTexture movieD1;
    public MovieTexture movieA3;
    public MovieTexture movieB3;
    public MovieTexture movieC4;
    public MovieTexture movieD2;
    public MovieTexture movieA4;
    public MovieTexture movieB4;
    public MovieTexture movieC5;
    public MovieTexture movieD3;
    public MovieTexture movieA5;
    public MovieTexture movieB5;
    public MovieTexture movieC6;
    public MovieTexture endBlack;
    //public MovieTexture movieA6;
    //public MovieTexture movieB6;
    //public MovieTexture movieC7;

    //public AudioSource[] audio;

    private AudioSource movieAudio;
    private AudioClip movieAudioClip;
    public AudioClip bgAudio;

    private IdeonGestureListener gestureListener;
    private int leanLeftCount;
    private int leanRightCount;
    private int shoulderLeftFrontCount;
    private int shoulderRightFrontCount;
    private int zoomInCount;
    private int zoomOutCount;
    private int whichMoviePlaying;

    private char[] branch;
    private int branchIndex;
    private string branchList;

    private BgAudio backgroundAudio;

    private bool timerOn;
    private float timeNoUser;
    private float timeForAUser;
    // initialization
    void Start () {

        backgroundAudio = BgAudio.Instance;
        //backgroundAudio.Play();
        movieAudio = GetComponent<AudioSource>();
        //movieAudio.PlayOneShot(bgAudio, 1.0F);
        //BgAudio bgAudio = new BgAudio();
        //bgAudio.PlayBG();

        sp.Open();
        // hide mouse cursor
        //Cursor.visible = false;
        branchList = "";
        branchIndex = 0;
        branch = new char[6];
        ResetCounts();
        playMovie(endBlack);
        timerOn = false;
        timeNoUser = 0;
        timeForAUser = 0;
        //audio_bgm.Play();
        whichMoviePlaying = 12;
        if (videoInfo != null)
        {
            //videoInfo.GetComponent<GUIText>().text = "Lean left, right to change the slides.";
        }
        
        // get the gestures listener
        gestureListener = IdeonGestureListener.Instance;
    }
	
	void Update () {
        // dont run Update() if there is no gesture listener
        if (!gestureListener)
            return;

        if (!gestureListener.currentUser())
        {
            timerOn = true;
        } else {
            timerOn = false;
            timeForAUser += Time.deltaTime;
        }

        if (timerOn)
        {
            timeNoUser += Time.deltaTime;
        }

        if (timeNoUser > 5)
        {
            whichMoviePlaying = 11;
            Debug.Log("This user spent " + timeForAUser.ToString("n2") + " seconds in the space");
            timeForAUser = 0;
            timeNoUser = 0;
        }
        //videoInfo.GetComponent<GUIText>().text = leanRightCount.ToString();
        if(whichMoviePlaying == 1)
        {
            if (movieC1.isPlaying)
            {
                CaptureGuestures();
                sp.Write("A");
            }
            else
            {
                ClipEndAction(1, movieA1, movieB1, 1);
            }
        }
        else if(whichMoviePlaying == 2)
        {
            if(!movieA1.isPlaying && !movieB1.isPlaying)
            {
                playMovie(movieC2);
                whichMoviePlaying = 3;
            }
        }
        else if (whichMoviePlaying == 3)
        {
            if (movieC2.isPlaying)
            {
                sp.Write("F");
                CaptureGuestures();
            }
            else
            {
                //CompletelyEndAction();
                ClipEndAction(3, movieA2, movieB2, 2);
            }
        }
        else if (whichMoviePlaying == 4)
        {
            if (!movieA2.isPlaying && !movieB2.isPlaying)
            {
                playMovie(movieC3);
                whichMoviePlaying = 5;
            }
        }
        else if (whichMoviePlaying == 5)
        {
            if (movieC3.isPlaying)
            {
                sp.Write("F");
                CaptureGuestures();
            }
            else
            {
                ClipEndActionWithMovieD(5, movieA3, movieB3, movieD1, 3);
            }
        }
        else if (whichMoviePlaying == 6)
        {
            if (!movieA3.isPlaying && !movieB3.isPlaying && !movieD1.isPlaying)
            {
                playMovie(movieC4);
                whichMoviePlaying = 7;
            }
        }
        else if (whichMoviePlaying == 7)
        {
            if (movieC4.isPlaying)
            {
                sp.Write("F");
                CaptureGuestures();
            }
            else
            {
                ClipEndActionWithMovieD(7, movieA4, movieB4, movieD2, 4);
            }
        }
        else if (whichMoviePlaying == 8)
        {
            if (!movieA4.isPlaying && !movieB4.isPlaying && !movieD2.isPlaying)
            {
                playMovie(movieC5);
                whichMoviePlaying = 9;
            }
        }
        else if (whichMoviePlaying == 9)
        {
            if (movieC5.isPlaying)
            {
                sp.Write("F");
                CaptureGuestures();
            }
            else
            {
                ClipEndActionWithMovieD(9, movieA5, movieB5, movieD3, 5);
            }
        }
        else if (whichMoviePlaying == 10)
         {
             if (!movieA5.isPlaying && !movieB5.isPlaying && !movieD3.isPlaying)
             {
                 playMovie(movieC6);
                 whichMoviePlaying = 11;
             }
         }
         else if (whichMoviePlaying == 11)
         {
            if (!movieC6.isPlaying)
            {
                playMovie(endBlack);
                whichMoviePlaying = 12;
            } else
            {
                sp.Write("A");
            }
        }
        else if (whichMoviePlaying == 12)
        {
            if (!endBlack.isPlaying)
            {
                CompletelyEndAction();
            }
            else
            {
                gestureListener.ResetGesture();
            }
        }

        /*
         else if (whichMoviePlaying == 12)
         {
             if (!movieA6.isPlaying && !movieB6.isPlaying)
             {
                 playMovie(movieC7);
                 whichMoviePlaying = 13;
             }
         }

         else if(whichMoviePlaying == 13)
         {
             if (!movieC7.isPlaying)
             {
                 CompletelyEndAction();
             }
         }
         */

        // Keyboard events

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jumping to start");
            branchIndex = 0;
            videoInfo.GetComponent<GUIText>().text = " ";
            ResetCounts();
            playMovie(endBlack);
            whichMoviePlaying = 12;
            branchList = "";
            backgroundAudio.Stop();
            backgroundAudio.Play();
        }
        
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        /* else if (Input.GetKeyDown(KeyCode.Return))
        {
            GetComponent<RawImage>().texture = movie1 as MovieTexture;
            movie1.Play();
        }
        */
    }

    private void CaptureGuestures()
    {
        if (gestureListener)
        {
            if (gestureListener.IsLeanLeft())
            {
                leanLeftCount++;
            }
            if (gestureListener.IsLeanRight())
            {
                leanRightCount++;
            }
            if (gestureListener.IsShoulderLeftFront())
            {
                shoulderLeftFrontCount++;
            }
            if (gestureListener.IsShoulderRightFront())
            {
                shoulderRightFrontCount++;
            }
            if (gestureListener.IsZoomIn())
            {
                zoomInCount++;
            }
            if (gestureListener.IsZoomOut())
            {
                zoomOutCount++;
            }
        }
    }

    private void ResetCounts()
    {
        leanLeftCount = 0;
        leanRightCount = 0;
        shoulderLeftFrontCount = 0;
        shoulderRightFrontCount = 0;
        zoomInCount = 0;
        zoomOutCount = 0;
    }

    private void playMovie(MovieTexture movie)
    {
        GetComponent<RawImage>().texture = movie as MovieTexture;
        movieAudio = GetComponent<AudioSource>();
        //movieAudioClip = movie.audioClip;
        movie.Stop();
        movie.Play();
        movieAudio.clip = movie.audioClip;
        movieAudio.Play(); 
    }

    private void ClipEndAction(int currentMovie, MovieTexture movieA, MovieTexture movieB, int mode)
    {
        
        if (mode == 1)
        {
            if (leanLeftCount > 0 || leanRightCount > 0)
            {
                Debug.Log("lean");
                playMovie(movieA);
                branch[branchIndex] = 'A';
                sp.Write("I");
            }
            else
            {
                Debug.Log("no lean");
                playMovie(movieB);
                branch[branchIndex] = 'B';
                sp.Write("C");
            }
           
            //Debug.Log(branch.ToString());
            
        }
        if (mode == 2)
        {
            if (shoulderLeftFrontCount > 0 || shoulderRightFrontCount > 0)
            {
                Debug.Log("lean");
                playMovie(movieA);
                branch[branchIndex] = 'A';
                sp.Write("E");
            }
            else
            {
                Debug.Log("no lean");
                playMovie(movieB);
                branch[branchIndex] = 'B';
                sp.Write("I");
            }
        }
        branchIndex++;
        whichMoviePlaying = currentMovie + 1;
        ResetCounts();
    }

    private void ClipEndActionWithMovieD(int currentMovie, MovieTexture movieA, MovieTexture movieB, MovieTexture movieD, int mode)
    {

        if (mode == 3)
        {
            if (shoulderLeftFrontCount > 0 || shoulderRightFrontCount > 0)
            {
                Debug.Log("turn around");
                playMovie(movieA);
                branch[branchIndex] = 'A';
                sp.Write("G");
            }
            else if (leanLeftCount > 0 || leanRightCount > 0)
            {
                Debug.Log("lean");
                playMovie(movieB);
                branch[branchIndex] = 'B';
                sp.Write("C");
            } else
            {
                Debug.Log("no lean");
                playMovie(movieD);
                branch[branchIndex] = 'D';
                sp.Write("H");
            }
        }
        if (mode == 4)
        {
            if (shoulderLeftFrontCount > 0 || shoulderRightFrontCount > 0)
            {
                Debug.Log("turn around");
                playMovie(movieA);
                branch[branchIndex] = 'A';
                sp.Write("E");
            }
            else if (leanLeftCount > 0 || leanRightCount > 0)
            {
                Debug.Log("lean");
                playMovie(movieB);
                branch[branchIndex] = 'B';
                sp.Write("B");
            }
            else
            {
                Debug.Log("no lean");
                playMovie(movieD);
                branch[branchIndex] = 'D';
                sp.Write("A");
            }
        }
        if (mode == 5)
        {
            if (shoulderLeftFrontCount > 0 || shoulderRightFrontCount > 0)
            {
                Debug.Log("turn around");
                playMovie(movieA);
                branch[branchIndex] = 'A';
                sp.Write("G");

            }
            else if (leanLeftCount > 0 || leanRightCount > 0)
            {
                Debug.Log("lean");
                playMovie(movieB);
                branch[branchIndex] = 'B';
                sp.Write("C");
            }
            else
            {
                Debug.Log("no lean");
                playMovie(movieD);
                branch[branchIndex] = 'D';
                sp.Write("I");
            }
        }
        branchIndex++;
        whichMoviePlaying = currentMovie + 1;
        ResetCounts();
    }

    private void CompletelyEndAction()
    {
        if(branchList == "")
        {
            foreach (char onebranch in branch)
            {
                branchList += onebranch;
            }
        }
        Debug.Log(branchList);
        //videoInfo.GetComponent<GUIText>().text = string.Format("{0} Swipe left to go back to start", branchList);
        string Videoinfo =  " Swipe left to start";
        videoInfo.GetComponent<GUIText>().text = Videoinfo;
        if (gestureListener.IsSwipeLeft())
        {
            Debug.Log("Jumping to start");
            branchIndex = 0;
            videoInfo.GetComponent<GUIText>().text = " ";
            ResetCounts();
            playMovie(movieC1);
            whichMoviePlaying = 1;
            branchList = "";
            backgroundAudio.Stop();
            backgroundAudio.Play();
            timeNoUser = 0;
        }
    }
}
