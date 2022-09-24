# CurvedShapeCalculator

This was a calculator that I created for an obscure dimensioning problem at work.  We had an odd shaped piece that the customer had requested, however they gave the dimensions in an uncommon way.

The app takes in those inputs and finds out the length that is required to actually design the piece.  Unfortunatley, there was no direct way to do this with a mathematical formula, so the solution itterates over a trial and error process, constantly refining its result until it gets to the expected answer.  This usually only takes 15-30 attempts, and the software can do that in a split second vs. trying to manually draft up each attempt one by one.

I created the UI un unity as practice, but it when exported to a webgl release build, the resolution changed, and a few things are not as clean as I would have liked.  It was baiscally just intended as an interface to access the code though.

https://play.unity.com/mg/other/curvedshapecalculator


