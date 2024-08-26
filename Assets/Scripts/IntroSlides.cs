using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSlides : MonoBehaviour
{
    [SerializeField] RawImage[] introImages;
    [SerializeField] RawImage titleImages;


    [SerializeField] float titleTime = 2f;
    [SerializeField] float displayTime = 12f;

    private int currentImageIndex = 0;
    private float timer = 0f;
    private bool _isSlideshowStart = false;

    void Awake()
    {
        // Hide all images initially except the title
        titleImages.gameObject.SetActive(true);
        foreach (RawImage img in introImages)
        {
            img.gameObject.SetActive(false);
        }

        // Show the first image
        /*if (introImages.Length > 0)
        {
            introImages[0].gameObject.SetActive(true);
        }*/
    }

    void Update()
    {
        // Skip to next if D is pressed
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(_isSlideshowStart)
            {
                ShowNextImage();
                timer = 0f;
            }
            else
            {
                titleImages.gameObject.SetActive(false);
                introImages[0].gameObject.SetActive(true);
                timer = 0f;
                _isSlideshowStart = true;
            }
        }
        // Skip the intro if the skip key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipIntro();
            return;
        }

        // Update timer
        timer += Time.deltaTime;
        // Check if it's time to show the next image
        if (_isSlideshowStart)
        {
            if (timer >= displayTime)
                    {
                        timer = 0f;
                        ShowNextImage();
                    }
        }
        else
        {
            if(timer >= titleTime)
            {
                titleImages.gameObject.SetActive(false);
                introImages[0].gameObject.SetActive(true);
                timer = 0f;
                _isSlideshowStart = true;
            }
        }
        
        
    }

    void ShowNextImage()
    {
        // Hide the current image
        if (currentImageIndex < introImages.Length)
        {
            introImages[currentImageIndex].gameObject.SetActive(false);
        }

        // Increment the image index
        currentImageIndex++;

        // Show the next image
        if (currentImageIndex < introImages.Length)
        {
            introImages[currentImageIndex].gameObject.SetActive(true);
        }
        else
        {
            // All images have been shown, start the game
            EndIntro();
        }
    }
    private void SkipIntro()
    {
        // Hide all images
        foreach (RawImage img in introImages)
        {
            img.gameObject.SetActive(false);
        }

        // End the intro immediately
        EndIntro();
    }

    void EndIntro()
    {
        Debug.Log("Intro Ended. Starting the game...");
        gameObject.SetActive(false);
    }
}
