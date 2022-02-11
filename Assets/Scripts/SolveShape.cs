using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SolveShape : MonoBehaviour
{
    //User Supplied Variables
    public float heightOfShape = 0f;
    public float lengthOfBottom = 0f;
    public float lengthOfSides = 0f;
    public float targetLenghtOfArc = 0f;
    public float startingAngle = 45f;
    public float angleAdjustmentAmount = 5f;

    //variables to be calculated
    float topAngle = 0f;
    float bottomAngle = 0f;
    float heightOfTriangle = 0f;
    float baseOfTriangle = 0f;
    float heightOfArc = 0f;
    float cordLength = 0f;
    float radiusOfArc = 0f;
    float arcLength = 0f;
    
    float finalWidthOfPiece = 0f;
    float finalArcLength = 0f;
    
    float guessLowAngle;
    float guessHighAngle;
    public int attemptCounter = 0;
    [SerializeField] public int numberOfAttemptsAllowed = 100;

    [SerializeField] TMP_InputField hInput;
    [SerializeField] TMP_InputField wInput;
    [SerializeField] TMP_InputField lInput;
    [SerializeField] TMP_InputField aInput;
    [SerializeField] TMP_InputField startingAngleInput;
    [SerializeField] TMP_InputField angleAdjustmentAmountInput;

    [SerializeField] TextMeshProUGUI solutionText;
    [SerializeField] TextMeshProUGUI sAText;
    [SerializeField] TextMeshProUGUI sBText;
    [SerializeField] TextMeshProUGUI aAText;
    [SerializeField] TextMeshProUGUI aBText;
    [SerializeField] TextMeshProUGUI PText;
    [SerializeField] TextMeshProUGUI RAText;
            
    // Start is called before the first frame update
    void Start()
    {
        //SolveShapeAndDisplayResults();
    }

    public void SolveShapeAndDisplayResults()
    {
        finalArcLength = FindTargetCordLength();
        UpdateTextFieldsWithResults();
    }
    
    private float FindTargetCordLength()
    {
        attemptCounter = 0;
        guessLowAngle = startingAngle;
        guessHighAngle = guessLowAngle + angleAdjustmentAmount;

        string evaluationResult = "";
                
        for (int i = 0; i < numberOfAttemptsAllowed; i++)
        {
            attemptCounter++;
            
            float guessLowArcLength =  FindArcLengthOfShapeGivenAngle(guessLowAngle);
            float guessHighArcLength =  FindArcLengthOfShapeGivenAngle(guessHighAngle);
             
            evaluationResult = EvaluateGuesses(guessLowArcLength, guessHighArcLength);
            
            if (evaluationResult == "matchLow")
            {return FindArcLengthOfShapeGivenAngle(guessLowAngle);}
            else if (evaluationResult == "matchHigh")
            {return FindArcLengthOfShapeGivenAngle(guessHighAngle);}
            else if (evaluationResult == "isBetween")
            {AdjustHighOrLowGuessBasedOnMedianAngle();}
            else if (evaluationResult == "bothBelow")
            {AdjustBothGuessAmountsByValue(angleAdjustmentAmount);}
            else if(evaluationResult == "bothAbove")
            {AdjustBothGuessAmountsByValue(angleAdjustmentAmount * -1);}            
        }
    
        return 0f;
    }
    
    private string EvaluateGuesses(float guessLowArcLength, float guessHighArcLength)
    {
        //Does guess match target if so return "Match"
        if(targetLenghtOfArc == System.Math.Round(guessLowArcLength,3))
        {return "matchLow";}
        else if(targetLenghtOfArc == System.Math.Round(guessHighArcLength,3))
        {return "matchHigh";}
        else if (guessLowArcLength < targetLenghtOfArc && targetLenghtOfArc < guessHighArcLength)
        {return "isBetween";}
        else if (targetLenghtOfArc > guessLowArcLength)
        {return "bothBelow";}
        else if(targetLenghtOfArc < guessHighArcLength)
        {return "bothAbove";}
        else
        {return "error";}
    }

    private void AdjustHighOrLowGuessBasedOnMedianAngle()
    {
        float medianAngle = guessLowAngle + (guessHighAngle - guessLowAngle) / 2;
        float medianArcLength = FindArcLengthOfShapeGivenAngle(medianAngle);

        if (targetLenghtOfArc > medianArcLength)
        {guessLowAngle = medianAngle;}
        else if (targetLenghtOfArc < medianArcLength)
        {guessHighAngle = medianAngle;}
    }
    
    private void AdjustBothGuessAmountsByValue(float valueToAdjust)
    {
        guessLowAngle = guessLowAngle + valueToAdjust;
        guessHighAngle = guessHighAngle + valueToAdjust;
    }
    
    private float FindArcLengthOfShapeGivenAngle(float angleToTry)
    {
        FindCordLengthOfShapeGivenAngle(angleToTry);
        
        radiusOfArc = FindRadiusOfArcGivenCordLengthAndHeightOfArc(cordLength, heightOfArc);
        arcLength = FindArcLengthGivenCordLengthAndRadiusOfArc(cordLength,radiusOfArc);
                
        return arcLength;
    }

    private float FindCordLengthOfShapeGivenAngle(float angleToTry)
    {
        FindTrinagleBaseAndHeight(angleToTry);
        FindHeightOfArcAndCordLength();
        
        return cordLength;
    }

    private void FindTrinagleBaseAndHeight(float angleToTry)
    {
        topAngle = angleToTry;
        bottomAngle = 90-topAngle;
                
        heightOfTriangle = FindLengthOfSideOfTriangleGiven2AnglesAndASide(lengthOfSides,bottomAngle, 90);
        baseOfTriangle = FindLengthOfSideOfTriangleGiven2AnglesAndASide(lengthOfSides,topAngle, 90);
    }

    private void FindHeightOfArcAndCordLength()
    {
        heightOfArc = heightOfShape - heightOfTriangle;
        cordLength = lengthOfBottom + baseOfTriangle + baseOfTriangle;
    }

    private float FindLengthOfSideOfTriangleGiven2AnglesAndASide(float lengthOfGivenSide, float adjacentAngleInDegrees, float oppositeAngleInDegrees)
    {
        float oppositeAngleInRaidans = ConvertDegreeToRadians(oppositeAngleInDegrees);
        float adjacentAngleInRaidans = ConvertDegreeToRadians(adjacentAngleInDegrees);
        
        float lengthOfSide = (lengthOfGivenSide * (Mathf.Sin(adjacentAngleInRaidans) / Mathf.Sin(oppositeAngleInRaidans) ));

        return lengthOfSide;
    }

    private float FindArcLengthGivenCordLengthAndRadiusOfArc(float cordLength, float radiusOfArc)
    {
        float arcLength = ((Mathf.Asin(cordLength / (radiusOfArc * 2))) * 2) * radiusOfArc;
        return arcLength;
    }
    
    private float FindRadiusOfArcGivenCordLengthAndHeightOfArc(float cordLength, float heightOfArc)
    {
        float radiusOfArc = (((cordLength*cordLength) / (heightOfArc*4)) + heightOfArc) / 2;
        return radiusOfArc;
    }

    private float ConvertDegreeToRadians(float degree)
    {
        float radians = degree * (Mathf.PI/180);
        return radians;
    }

    public void SetHeightOfShape()
    {heightOfShape = float.Parse(hInput.text);}
    
    public void SetlengthOfBottom()
    {lengthOfBottom = float.Parse(wInput.text);}

    public void SetLengthOfSides()
    {lengthOfSides = float.Parse(lInput.text);}

    public void SetTargetLengthOfArc()
    {targetLenghtOfArc =  float.Parse(aInput.text);}    

    public void SetStartingAngleInput()
    {startingAngle =  float.Parse(startingAngleInput.text);}

    public void SetAngleAdjustmentAmount()
    {angleAdjustmentAmount =  float.Parse(angleAdjustmentAmountInput.text);}

    public void UpdateTextFieldsWithResults()
    {
        SetSolutionText();
        SetsAText();
        SetsBText();
        SetaAText();
        SetaBText();
        SetPText();
        SetRAText();
    }

    public void SetSolutionText()
    {solutionText.text = "Solution In " + attemptCounter + " tries\nUnknown = " + System.Math.Round(cordLength,3);}

    public void SetsAText()
    {sAText.text = "sA = " + System.Math.Round(heightOfTriangle,3);}

    public void SetsBText()
    {sBText.text = "sB = " + System.Math.Round(baseOfTriangle,3);}

    public void SetaAText()
    {aAText.text = "aA = " + System.Math.Round(bottomAngle,3);}

    public void SetaBText()
    {aBText.text = "aB = " + System.Math.Round(topAngle,3);}

    public void SetPText()
    {PText.text = "P = " + System.Math.Round(heightOfArc,3);}

    public void SetRAText()
    {RAText.text = "R = " + System.Math.Round(radiusOfArc,3);}

}
