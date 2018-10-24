Imports System.Runtime.InteropServices
Imports LeptonicaSharp.Enumerations
Partial Public Class _All


' SRC\convolve.c (127, 1)
' pixBlockconv(pix, wc, hc) as Pix
' pixBlockconv(PIX *, l_int32, l_int32) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) The full width and height of the convolution kernel<para/>
''' are (2  wc + 1) and (2  hc + 1)<para/>
''' (2) Returns a copy if both wc and hc are 0<para/>
''' (3) Require that w  is greater = 2  wc + 1 and h  is greater = 2  hc + 1,<para/>
''' where (w,h) are the dimensions of pixs.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pix">[in] - 8 or 32 bpp or 2, 4 or 8 bpp with colormap</param>
'''  <param name="wc">[in] - half width/height of convolution kernel</param>
'''  <param name="hc">[in] - half width/height of convolution kernel</param>
'''   <returns>pixd, or NULL on error</returns>
Public Shared Function pixBlockconv(
				 ByVal pix as Pix, 
				 ByVal wc as Integer, 
				 ByVal hc as Integer) as Pix

	If IsNothing (pix) then Throw New ArgumentNullException  ("pix cannot be Nothing")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixBlockconv( pix.Pointer, wc, hc)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (210, 1)
' pixBlockconvGray(pixs, pixacc, wc, hc) as Pix
' pixBlockconvGray(PIX *, PIX *, l_int32, l_int32) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) If accum pix is null, make one and destroy it before<para/>
''' returning otherwise, just use the input accum pix.<para/>
''' (2) The full width and height of the convolution kernel<para/>
''' are (2  wc + 1) and (2  hc + 1).<para/>
''' (3) Returns a copy if both wc and hc are 0.<para/>
''' (4) Require that w  is greater = 2  wc + 1 and h  is greater = 2  hc + 1,<para/>
''' where (w,h) are the dimensions of pixs.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 8 bpp</param>
'''  <param name="pixacc">[in] - pix 32 bpp can be null</param>
'''  <param name="wc">[in] - half width/height of convolution kernel</param>
'''  <param name="hc">[in] - half width/height of convolution kernel</param>
'''   <returns>pix 8 bpp, or NULL on error</returns>
Public Shared Function pixBlockconvGray(
				 ByVal pixs as Pix, 
				 ByVal pixacc as Pix, 
				 ByVal wc as Integer, 
				 ByVal hc as Integer) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")
	If IsNothing (pixacc) then Throw New ArgumentNullException  ("pixacc cannot be Nothing")

	If {8}.contains (pixs.d) = false then Throw New ArgumentException ("8 bpp")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixBlockconvGray( pixs.Pointer, pixacc.Pointer, wc, hc)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (455, 1)
' pixBlockconvAccum(pixs) as Pix
' pixBlockconvAccum(PIX *) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) The general recursion relation is<para/>
''' a(i,j) = v(i,j) + a(i-1, j) + a(i, j-1) - a(i-1, j-1)<para/>
''' For the first line, this reduces to the special case<para/>
''' a(i,j) = v(i,j) + a(i, j-1)<para/>
''' For the first column, the special case is<para/>
''' a(i,j) = v(i,j) + a(i-1, j)<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 1, 8 or 32 bpp</param>
'''   <returns>accum pix 32 bpp, or NULL on error.</returns>
Public Shared Function pixBlockconvAccum(
				 ByVal pixs as Pix) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixBlockconvAccum( pixs.Pointer)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (636, 1)
' pixBlockconvGrayUnnormalized(pixs, wc, hc) as Pix
' pixBlockconvGrayUnnormalized(PIX *, l_int32, l_int32) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) The full width and height of the convolution kernel<para/>
''' are (2  wc + 1) and (2  hc + 1).<para/>
''' (2) Require that w  is greater = 2  wc + 1 and h  is greater = 2  hc + 1,<para/>
''' where (w,h) are the dimensions of pixs.<para/>
''' (3) Returns a copy if both wc and hc are 0.<para/>
''' (3) Adds mirrored border to avoid treating the boundary pixels<para/>
''' specially.  Note that we add wc + 1 pixels to the left<para/>
''' and wc to the right.  The added width is 2  wc + 1 pixels,<para/>
''' and the particular choice simplifies the indexing in the loop.<para/>
''' Likewise, add hc + 1 pixels to the top and hc to the bottom.<para/>
''' (4) To get the normalized result, divide by the area of the<para/>
''' convolution kernel: (2  wc + 1)  (2  hc + 1)<para/>
''' Specifically, do this:<para/>
''' pixc = pixBlockconvGrayUnnormalized(pixs, wc, hc)<para/>
''' fract = 1. / ((2  wc + 1)  (2  hc + 1))<para/>
''' pixMultConstantGray(pixc, fract)<para/>
''' pixd = pixGetRGBComponent(pixc, L_ALPHA_CHANNEL)<para/>
''' (5) Unlike pixBlockconvGray(), this always computes the accumulation<para/>
''' pix because its size is tied to wc and hc.<para/>
''' (6) Compare this implementation with pixBlockconvGray(), where<para/>
''' most of the code in blockconvLow() is special casing for<para/>
''' efficiently handling the boundary.  Here, the use of<para/>
''' mirrored borders and destination indexing makes the<para/>
''' implementation very simple.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 8 bpp</param>
'''  <param name="wc">[in] - half width/height of convolution kernel</param>
'''  <param name="hc">[in] - half width/height of convolution kernel</param>
'''   <returns>pix 32 bpp containing the convolution without normalizing for the window size, or NULL on error</returns>
Public Shared Function pixBlockconvGrayUnnormalized(
				 ByVal pixs as Pix, 
				 ByVal wc as Integer, 
				 ByVal hc as Integer) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")

	If {8}.contains (pixs.d) = false then Throw New ArgumentException ("8 bpp")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixBlockconvGrayUnnormalized( pixs.Pointer, wc, hc)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (727, 1)
' pixBlockconvTiled(pix, wc, hc, nx, ny) as Pix
' pixBlockconvTiled(PIX *, l_int32, l_int32, l_int32, l_int32) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) The full width and height of the convolution kernel<para/>
''' are (2  wc + 1) and (2  hc + 1)<para/>
''' (2) Returns a copy if both wc and hc are 0<para/>
''' (3) Require that w  is greater = 2  wc + 1 and h  is greater = 2  hc + 1,<para/>
''' where (w,h) are the dimensions of pixs.<para/>
''' (4) For nx == ny == 1, this defaults to pixBlockconv(), which<para/>
''' is typically about twice as fast, and gives nearly<para/>
''' identical results as pixBlockconvGrayTile().<para/>
''' (5) If the tiles are too small, nx and/or ny are reduced<para/>
''' a minimum amount so that the tiles are expanded to the<para/>
''' smallest workable size in the problematic direction(s).<para/>
''' (6) Why a tiled version?  Three reasons:<para/>
''' (a) Because the accumulator is a uint32, overflow can occur<para/>
''' for an image with more than 16M pixels.<para/>
''' (b) The accumulator array for 16M pixels is 64 MB using<para/>
''' tiles reduces the size of this array.<para/>
''' (c) Each tile can be processed independently, in parallel,<para/>
''' on a multicore processor.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pix">[in] - 8 or 32 bpp or 2, 4 or 8 bpp with colormap</param>
'''  <param name="wc">[in] - half width/height of convolution kernel</param>
'''  <param name="hc">[in] - half width/height of convolution kernel</param>
'''  <param name="nx">[in] - subdivision into tiles</param>
'''  <param name="ny">[in] - subdivision into tiles</param>
'''   <returns>pixd, or NULL on error</returns>
Public Shared Function pixBlockconvTiled(
				 ByVal pix as Pix, 
				 ByVal wc as Integer, 
				 ByVal hc as Integer, 
				 ByVal nx as Integer, 
				 ByVal ny as Integer) as Pix

	If IsNothing (pix) then Throw New ArgumentNullException  ("pix cannot be Nothing")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixBlockconvTiled( pix.Pointer, wc, hc, nx, ny)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (853, 1)
' pixBlockconvGrayTile(pixs, pixacc, wc, hc) as Pix
' pixBlockconvGrayTile(PIX *, PIX *, l_int32, l_int32) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) The full width and height of the convolution kernel<para/>
''' are (2  wc + 1) and (2  hc + 1)<para/>
''' (2) Assumes that the input pixs is padded with (wc + 1) pixels on<para/>
''' left and right, and with (hc + 1) pixels on top and bottom.<para/>
''' The returned pix has these stripped off they are only used<para/>
''' for computation.<para/>
''' (3) Returns a copy if both wc and hc are 0<para/>
''' (4) Require that w  is greater  2  wc + 1 and h  is greater  2  hc + 1,<para/>
''' where (w,h) are the dimensions of pixs.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 8 bpp gray</param>
'''  <param name="pixacc">[in] - 32 bpp accum pix</param>
'''  <param name="wc">[in] - half width/height of convolution kernel</param>
'''  <param name="hc">[in] - half width/height of convolution kernel</param>
'''   <returns>pixd, or NULL on error</returns>
Public Shared Function pixBlockconvGrayTile(
				 ByVal pixs as Pix, 
				 ByVal pixacc as Pix, 
				 ByVal wc as Integer, 
				 ByVal hc as Integer) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")
	If IsNothing (pixacc) then Throw New ArgumentNullException  ("pixacc cannot be Nothing")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixBlockconvGrayTile( pixs.Pointer, pixacc.Pointer, wc, hc)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (980, 1)
' pixWindowedStats(pixs, wc, hc, hasborder, ppixm, ppixms, pfpixv, pfpixrv) as Integer
' pixWindowedStats(PIX *, l_int32, l_int32, l_int32, PIX **, PIX **, FPIX **, FPIX **) as l_ok
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) This is a high-level convenience function for calculating<para/>
''' any or all of these derived images.<para/>
''' (2) If %hasborder = 0, a border is added and the result is<para/>
''' computed over all pixels in pixs.  Otherwise, no border is<para/>
''' added and the border pixels are removed from the output images.<para/>
''' (3) These statistical measures over the pixels in the<para/>
''' rectangular window are:<para/>
''' ~ average value:  is lower p is greater (pixm)<para/>
''' ~ average squared value:  is lower pp is greater  (pixms)<para/>
''' ~ variance:  is lower (p -  is lower p is greater )(p -  is lower p is greater ) is greater  =  is lower pp is greater  -  is lower p is greater  is lower p is greater (pixv)<para/>
''' ~ square-root of variance: (pixrv)<para/>
''' where the brackets  is lower  ..  is greater  indicate that the average value is<para/>
''' to be taken over the window.<para/>
''' (4) Note that the variance is just the mean square difference from<para/>
''' the mean value and the square root of the variance is the<para/>
''' root mean square difference from the mean, sometimes also<para/>
''' called the 'standard deviation'.<para/>
''' (5) The added border, along with the use of an accumulator array,<para/>
''' allows computation without special treatment of pixels near<para/>
''' the image boundary, and runs in a time that is independent<para/>
''' of the size of the convolution kernel.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 8 bpp grayscale</param>
'''  <param name="wc">[in] - half width/height of convolution kernel</param>
'''  <param name="hc">[in] - half width/height of convolution kernel</param>
'''  <param name="hasborder">[in] - use 1 if it already has (wc + 1 border pixels on left and right, and hc + 1 on top and bottom use 0 to add kernel-dependent border)</param>
'''  <param name="ppixm">[out][optional] - 8 bpp mean value in window</param>
'''  <param name="ppixms">[out][optional] - 32 bpp mean square value in window</param>
'''  <param name="pfpixv">[out][optional] - float variance in window</param>
'''  <param name="pfpixrv">[out][optional] - float rms deviation from the mean</param>
'''   <returns>0 if OK, 1 on error</returns>
Public Shared Function pixWindowedStats(
				 ByVal pixs as Pix, 
				 ByVal wc as Integer, 
				 ByVal hc as Integer, 
				 ByVal hasborder as Integer, 
				<Out()> Optional ByRef ppixm as Pix = Nothing, 
				<Out()> Optional ByRef ppixms as Pix = Nothing, 
				<Out()> Optional ByRef pfpixv as FPix = Nothing, 
				<Out()> Optional ByRef pfpixrv as FPix = Nothing) as Integer

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")

	If {8}.contains (pixs.d) = false then Throw New ArgumentException ("8 bpp grayscale")

Dim ppixmPTR As IntPtr = IntPtr.Zero : If Not IsNothing(ppixm) Then ppixmPTR = ppixm.Pointer
Dim ppixmsPTR As IntPtr = IntPtr.Zero : If Not IsNothing(ppixms) Then ppixmsPTR = ppixms.Pointer
Dim pfpixvPTR As IntPtr = IntPtr.Zero : If Not IsNothing(pfpixv) Then pfpixvPTR = pfpixv.Pointer
Dim pfpixrvPTR As IntPtr = IntPtr.Zero : If Not IsNothing(pfpixrv) Then pfpixrvPTR = pfpixrv.Pointer

	Dim _Result as Integer = LeptonicaSharp.Natives.pixWindowedStats( pixs.Pointer, wc, hc, hasborder, ppixmPTR, ppixmsPTR, pfpixvPTR, pfpixrvPTR)
	if ppixmPTR <> IntPtr.Zero then ppixm = new Pix(ppixmPTR)
	if ppixmsPTR <> IntPtr.Zero then ppixms = new Pix(ppixmsPTR)
	if pfpixvPTR <> IntPtr.Zero then pfpixv = new FPix(pfpixvPTR)
	if pfpixrvPTR <> IntPtr.Zero then pfpixrv = new FPix(pfpixrvPTR)

	Return _Result
End Function

' SRC\convolve.c (1065, 1)
' pixWindowedMean(pixs, wc, hc, hasborder, normflag) as Pix
' pixWindowedMean(PIX *, l_int32, l_int32, l_int32, l_int32) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) The input and output depths are the same.<para/>
''' (2) A set of border pixels of width (wc + 1) on left and right,<para/>
''' and of height (hc + 1) on top and bottom, must be on the<para/>
''' pix before the accumulator is found.  The output pixd<para/>
''' (after convolution) has this border removed.<para/>
''' If %hasborder = 0, the required border is added.<para/>
''' (3) Typically, %normflag == 1.  However, if you want the sum<para/>
''' within the window, rather than a normalized convolution,<para/>
''' use %normflag == 0.<para/>
''' (4) This builds a block accumulator pix, uses it here, and<para/>
''' destroys it.<para/>
''' (5) The added border, along with the use of an accumulator array,<para/>
''' allows computation without special treatment of pixels near<para/>
''' the image boundary, and runs in a time that is independent<para/>
''' of the size of the convolution kernel.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 8 or 32 bpp grayscale</param>
'''  <param name="wc">[in] - half width/height of convolution kernel</param>
'''  <param name="hc">[in] - half width/height of convolution kernel</param>
'''  <param name="hasborder">[in] - use 1 if it already has (wc + 1 border pixels on left and right, and hc + 1 on top and bottom use 0 to add kernel-dependent border)</param>
'''  <param name="normflag">[in] - 1 for normalization to get average in window 0 for the sum in the window (un-normalized)</param>
'''   <returns>pixd 8 or 32 bpp, average over kernel window</returns>
Public Shared Function pixWindowedMean(
				 ByVal pixs as Pix, 
				 ByVal wc as Integer, 
				 ByVal hc as Integer, 
				 ByVal hasborder as Integer, 
				 ByVal normflag as Integer) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixWindowedMean( pixs.Pointer, wc, hc, hasborder, normflag)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (1182, 1)
' pixWindowedMeanSquare(pixs, wc, hc, hasborder) as Pix
' pixWindowedMeanSquare(PIX *, l_int32, l_int32, l_int32) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) A set of border pixels of width (wc + 1) on left and right,<para/>
''' and of height (hc + 1) on top and bottom, must be on the<para/>
''' pix before the accumulator is found.  The output pixd<para/>
''' (after convolution) has this border removed.<para/>
''' If %hasborder = 0, the required border is added.<para/>
''' (2) The advantage is that we are unaffected by the boundary, and<para/>
''' it is not necessary to treat pixels within %wc and %hc of the<para/>
''' border differently.  This is because processing for pixd<para/>
''' only takes place for pixels in pixs for which the<para/>
''' kernel is entirely contained in pixs.<para/>
''' (3) Why do we have an added border of width (%wc + 1) and<para/>
''' height (%hc + 1), when we only need %wc and %hc pixels<para/>
''' to satisfy this condition?  Answer: the accumulators<para/>
''' are asymmetric, requiring an extra row and column of<para/>
''' pixels at top and left to work accurately.<para/>
''' (4) The added border, along with the use of an accumulator array,<para/>
''' allows computation without special treatment of pixels near<para/>
''' the image boundary, and runs in a time that is independent<para/>
''' of the size of the convolution kernel.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 8 bpp grayscale</param>
'''  <param name="wc">[in] - half width/height of convolution kernel</param>
'''  <param name="hc">[in] - half width/height of convolution kernel</param>
'''  <param name="hasborder">[in] - use 1 if it already has (wc + 1 border pixels on left and right, and hc + 1 on top and bottom use 0 to add kernel-dependent border)</param>
'''   <returns>pixd 32 bpp, average over rectangular window of width = 2  wc + 1 and height = 2  hc + 1</returns>
Public Shared Function pixWindowedMeanSquare(
				 ByVal pixs as Pix, 
				 ByVal wc as Integer, 
				 ByVal hc as Integer, 
				 ByVal hasborder as Integer) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")

	If {8}.contains (pixs.d) = false then Throw New ArgumentException ("8 bpp grayscale")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixWindowedMeanSquare( pixs.Pointer, wc, hc, hasborder)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (1280, 1)
' pixWindowedVariance(pixm, pixms, pfpixv, pfpixrv) as Integer
' pixWindowedVariance(PIX *, PIX *, FPIX **, FPIX **) as l_ok
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) The mean and mean square values are precomputed, using<para/>
''' pixWindowedMean() and pixWindowedMeanSquare().<para/>
''' (2) Either or both of the variance and square-root of variance<para/>
''' are returned as an fpix, where the variance is the<para/>
''' average over the window of the mean square difference of<para/>
''' the pixel value from the mean:<para/>
'''  is lower (p -  is lower p is greater )(p -  is lower p is greater ) is greater  =  is lower pp is greater  -  is lower p is greater  is lower p is greater <para/>
''' (3) To visualize the results:<para/>
''' ~ for both, use fpixDisplayMaxDynamicRange().<para/>
''' ~ for rms deviation, simply convert the output fpix to pix,<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixm">[in] - mean over window 8 or 32 bpp grayscale</param>
'''  <param name="pixms">[in] - mean square over window 32 bpp</param>
'''  <param name="pfpixv">[out][optional] - float variance -- the ms deviation from the mean</param>
'''  <param name="pfpixrv">[out][optional] - float rms deviation from the mean</param>
'''   <returns>0 if OK, 1 on error</returns>
Public Shared Function pixWindowedVariance(
				 ByVal pixm as Pix, 
				 ByVal pixms as Pix, 
				<Out()> Optional ByRef pfpixv as FPix = Nothing, 
				<Out()> Optional ByRef pfpixrv as FPix = Nothing) as Integer

	If IsNothing (pixm) then Throw New ArgumentNullException  ("pixm cannot be Nothing")
	If IsNothing (pixms) then Throw New ArgumentNullException  ("pixms cannot be Nothing")

Dim pfpixvPTR As IntPtr = IntPtr.Zero : If Not IsNothing(pfpixv) Then pfpixvPTR = pfpixv.Pointer
Dim pfpixrvPTR As IntPtr = IntPtr.Zero : If Not IsNothing(pfpixrv) Then pfpixrvPTR = pfpixrv.Pointer

	Dim _Result as Integer = LeptonicaSharp.Natives.pixWindowedVariance( pixm.Pointer, pixms.Pointer, pfpixvPTR, pfpixrvPTR)
	if pfpixvPTR <> IntPtr.Zero then pfpixv = new FPix(pfpixvPTR)
	if pfpixrvPTR <> IntPtr.Zero then pfpixrv = new FPix(pfpixrvPTR)

	Return _Result
End Function

' SRC\convolve.c (1369, 1)
' pixMeanSquareAccum(pixs) as DPix
' pixMeanSquareAccum(PIX *) as DPIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) Similar to pixBlockconvAccum(), this computes the<para/>
''' sum of the squares of the pixel values in such a way<para/>
''' that the value at (i,j) is the sum of all squares in<para/>
''' the rectangle from the origin to (i,j).<para/>
''' (2) The general recursion relation (v are squared pixel values) is<para/>
''' a(i,j) = v(i,j) + a(i-1, j) + a(i, j-1) - a(i-1, j-1)<para/>
''' For the first line, this reduces to the special case<para/>
''' a(i,j) = v(i,j) + a(i, j-1)<para/>
''' For the first column, the special case is<para/>
''' a(i,j) = v(i,j) + a(i-1, j)<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 8 bpp grayscale</param>
'''   <returns>dpix 64 bit array, or NULL on error</returns>
Public Shared Function pixMeanSquareAccum(
				 ByVal pixs as Pix) as DPix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")

	If {8}.contains (pixs.d) = false then Throw New ArgumentException ("8 bpp grayscale")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixMeanSquareAccum( pixs.Pointer)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new DPix(_Result)
End Function

' SRC\convolve.c (1450, 1)
' pixBlockrank(pixs, pixacc, wc, hc, rank) as Pix
' pixBlockrank(PIX *, PIX *, l_int32, l_int32, l_float32) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) The full width and height of the convolution kernel<para/>
''' are (2  wc + 1) and (2  hc + 1)<para/>
''' (2) This returns a pixd where each pixel is a 1 if the<para/>
''' neighborhood (2  wc + 1) x (2  hc + 1)) pixels<para/>
''' contains the rank fraction of 1 pixels.  Otherwise,<para/>
''' the returned pixel is 0.  Note that the special case<para/>
''' of rank = 0.0 is always satisfied, so the returned<para/>
''' pixd has all pixels with value 1.<para/>
''' (3) If accum pix is null, make one, use it, and destroy it<para/>
''' before returning otherwise, just use the input accum pix<para/>
''' (4) If both wc and hc are 0, returns a copy unless rank == 0.0,<para/>
''' in which case this returns an all-ones image.<para/>
''' (5) Require that w  is greater = 2  wc + 1 and h  is greater = 2  hc + 1,<para/>
''' where (w,h) are the dimensions of pixs.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 1 bpp</param>
'''  <param name="pixacc">[in] - pix [optional] 32 bpp</param>
'''  <param name="wc">[in] - half width/height of block sum/rank kernel</param>
'''  <param name="hc">[in] - half width/height of block sum/rank kernel</param>
'''  <param name="rank">[in] - between 0.0 and 1.0 0.5 is median filter</param>
'''   <returns>pixd 1 bpp</returns>
Public Shared Function pixBlockrank(
				 ByVal pixs as Pix, 
				 ByVal pixacc as Pix, 
				 ByVal wc as Integer, 
				 ByVal hc as Integer, 
				 ByVal rank as Single) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")

	If {1}.contains (pixs.d) = false then Throw New ArgumentException ("1 bpp")

	Dim pixaccPTR As IntPtr = IntPtr.Zero : If Not IsNothing(pixacc) Then pixaccPTR = pixacc.Pointer

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixBlockrank( pixs.Pointer, pixaccPTR, wc, hc, rank)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (1532, 1)
' pixBlocksum(pixs, pixacc, wc, hc) as Pix
' pixBlocksum(PIX *, PIX *, l_int32, l_int32) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) If accum pix is null, make one and destroy it before<para/>
''' returning otherwise, just use the input accum pix<para/>
''' (2) The full width and height of the convolution kernel<para/>
''' are (2  wc + 1) and (2  hc + 1)<para/>
''' (3) Use of wc = hc = 1, followed by pixInvert() on the<para/>
''' 8 bpp result, gives a nice anti-aliased, and somewhat<para/>
''' darkened, result on text.<para/>
''' (4) Require that w  is greater = 2  wc + 1 and h  is greater = 2  hc + 1,<para/>
''' where (w,h) are the dimensions of pixs.<para/>
''' (5) Returns in each dest pixel the sum of all src pixels<para/>
''' that are within a block of size of the kernel, centered<para/>
''' on the dest pixel.  This sum is the number of src ON<para/>
''' pixels in the block at each location, normalized to 255<para/>
''' for a block containing all ON pixels.  For pixels near<para/>
''' the boundary, where the block is not entirely contained<para/>
''' within the image, we then multiply by a second normalization<para/>
''' factor that is greater than one, so that all results<para/>
''' are normalized by the number of participating pixels<para/>
''' within the block.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 1 bpp</param>
'''  <param name="pixacc">[in] - pix [optional] 32 bpp</param>
'''  <param name="wc">[in] - half width/height of block sum/rank kernel</param>
'''  <param name="hc">[in] - half width/height of block sum/rank kernel</param>
'''   <returns>pixd 8 bpp</returns>
Public Shared Function pixBlocksum(
				 ByVal pixs as Pix, 
				 ByVal pixacc as Pix, 
				 ByVal wc as Integer, 
				 ByVal hc as Integer) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")

	If {1}.contains (pixs.d) = false then Throw New ArgumentException ("1 bpp")

	Dim pixaccPTR As IntPtr = IntPtr.Zero : If Not IsNothing(pixacc) Then pixaccPTR = pixacc.Pointer

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixBlocksum( pixs.Pointer, pixaccPTR, wc, hc)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (1772, 1)
' pixCensusTransform(pixs, halfsize, pixacc) as Pix
' pixCensusTransform(PIX *, l_int32, PIX *) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) The Census transform was invented by Ramin Zabih and John Woodfill<para/>
''' ("Non-parametric local transforms for computing visual<para/>
''' correspondence", Third European Conference on Computer Vision,<para/>
''' Stockholm, Sweden, May 1994) see publications at<para/>
''' http://www.cs.cornell.edu/~rdz/index.htm<para/>
''' This compares each pixel against the average of its neighbors,<para/>
''' in a square of odd dimension centered on the pixel.<para/>
''' If the pixel is greater than the average of its neighbors,<para/>
''' the output pixel value is 1 otherwise it is 0.<para/>
''' (2) This can be used as an encoding for an image that is<para/>
''' fairly robust against slow illumination changes, with<para/>
''' applications in image comparison and mosaicing.<para/>
''' (3) The size of the convolution kernel is (2  halfsize + 1)<para/>
''' on a side.  The halfsize parameter must be  is greater = 1.<para/>
''' (4) If accum pix is null, make one, use it, and destroy it<para/>
''' before returning otherwise, just use the input accum pix<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 8 bpp</param>
'''  <param name="halfsize">[in] - of square over which neighbors are averaged</param>
'''  <param name="pixacc">[in] - pix [optional] 32 bpp</param>
'''   <returns>pixd 1 bpp</returns>
Public Shared Function pixCensusTransform(
				 ByVal pixs as Pix, 
				 ByVal halfsize as Integer, 
				 Optional ByVal pixacc as Pix = Nothing) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")

	If {8}.contains (pixs.d) = false then Throw New ArgumentException ("8 bpp")

	Dim pixaccPTR As IntPtr = IntPtr.Zero : If Not IsNothing(pixacc) Then pixaccPTR = pixacc.Pointer

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixCensusTransform( pixs.Pointer, halfsize, pixaccPTR)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (1872, 1)
' pixConvolve(pixs, kel, outdepth, normflag) as Pix
' pixConvolve(PIX *, L_KERNEL *, l_int32, l_int32) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) This gives a convolution with an arbitrary kernel.<para/>
''' (2) The input pixs must have only one sample/pixel.<para/>
''' To do a convolution on an RGB image, use pixConvolveRGB().<para/>
''' (3) The parameter %outdepth determines the depth of the result.<para/>
''' If the kernel is normalized to unit sum, the output values<para/>
''' can never exceed 255, so an output depth of 8 bpp is sufficient.<para/>
''' If the kernel is not normalized, it may be necessary to use<para/>
''' 16 or 32 bpp output to avoid overflow.<para/>
''' (4) If normflag == 1, the result is normalized by scaling all<para/>
''' kernel values for a unit sum.  If the sum of kernel values<para/>
''' is very close to zero, the kernel can not be normalized and<para/>
''' the convolution will not be performed.  A warning is issued.<para/>
''' (5) The kernel values can be positive or negative, but the<para/>
''' result for the convolution can only be stored as a positive<para/>
''' number.  Consequently, if it goes negative, the choices are<para/>
''' to clip to 0 or take the absolute value.  We're choosing<para/>
''' to take the absolute value.  (Another possibility would be<para/>
''' to output a second unsigned image for the negative values.)<para/>
''' If you want to get a clipped result, or to keep the negative<para/>
''' values in the result, use fpixConvolve(), with the<para/>
''' converters in fpix2.c between pix and fpix.<para/>
''' (6) This uses a mirrored border to avoid special casing on<para/>
''' the boundaries.<para/>
''' (7) To get a subsampled output, call l_setConvolveSampling().<para/>
''' The time to make a subsampled output is reduced by the<para/>
''' product of the sampling factors.<para/>
''' (8) The function is slow, running at about 12 machine cycles for<para/>
''' each pixel-op in the convolution.  For example, with a 3 GHz<para/>
''' cpu, a 1 Mpixel grayscale image, and a kernel with<para/>
''' (sx  sy) = 25 elements, the convolution takes about 100 msec.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 8, 16, 32 bpp no colormap</param>
'''  <param name="kel">[in] - kernel</param>
'''  <param name="outdepth">[in] - of pixd: 8, 16 or 32</param>
'''  <param name="normflag">[in] - 1 to normalize kernel to unit sum 0 otherwise</param>
'''   <returns>pixd 8, 16 or 32 bpp</returns>
Public Shared Function pixConvolve(
				 ByVal pixs as Pix, 
				 ByVal kel as L_Kernel, 
				 ByVal outdepth as Integer, 
				 ByVal normflag as Integer) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")
	If IsNothing (kel) then Throw New ArgumentNullException  ("kel cannot be Nothing")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixConvolve( pixs.Pointer, kel.Pointer, outdepth, normflag)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (2002, 1)
' pixConvolveSep(pixs, kelx, kely, outdepth, normflag) as Pix
' pixConvolveSep(PIX *, L_KERNEL *, L_KERNEL *, l_int32, l_int32) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) This does a convolution with a separable kernel that is<para/>
''' is a sequence of convolutions in x and y.  The two<para/>
''' one-dimensional kernel components must be input separately<para/>
''' the full kernel is the product of these components.<para/>
''' The support for the full kernel is thus a rectangular region.<para/>
''' (2) The input pixs must have only one sample/pixel.<para/>
''' To do a convolution on an RGB image, use pixConvolveSepRGB().<para/>
''' (3) The parameter %outdepth determines the depth of the result.<para/>
''' If the kernel is normalized to unit sum, the output values<para/>
''' can never exceed 255, so an output depth of 8 bpp is sufficient.<para/>
''' If the kernel is not normalized, it may be necessary to use<para/>
''' 16 or 32 bpp output to avoid overflow.<para/>
''' (2) The %normflag parameter is used as in pixConvolve().<para/>
''' (4) The kernel values can be positive or negative, but the<para/>
''' result for the convolution can only be stored as a positive<para/>
''' number.  Consequently, if it goes negative, the choices are<para/>
''' to clip to 0 or take the absolute value.  We're choosing<para/>
''' the former for now.  Another possibility would be to output<para/>
''' a second unsigned image for the negative values.<para/>
''' (5) Warning: if you use l_setConvolveSampling() to get a<para/>
''' subsampled output, and the sampling factor is larger than<para/>
''' the kernel half-width, it is faster to use the non-separable<para/>
''' version pixConvolve().  This is because the first convolution<para/>
''' here must be done on every raster line, regardless of the<para/>
''' vertical sampling factor.  If the sampling factor is smaller<para/>
''' than kernel half-width, it's faster to use the separable<para/>
''' convolution.<para/>
''' (6) This uses mirrored borders to avoid special casing on<para/>
''' the boundaries.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 8, 16, 32 bpp no colormap</param>
'''  <param name="kelx">[in] - x-dependent kernel</param>
'''  <param name="kely">[in] - y-dependent kernel</param>
'''  <param name="outdepth">[in] - of pixd: 8, 16 or 32</param>
'''  <param name="normflag">[in] - 1 to normalize kernel to unit sum 0 otherwise</param>
'''   <returns>pixd 8, 16 or 32 bpp</returns>
Public Shared Function pixConvolveSep(
				 ByVal pixs as Pix, 
				 ByVal kelx as L_Kernel, 
				 ByVal kely as L_Kernel, 
				 ByVal outdepth as Integer, 
				 ByVal normflag as Integer) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")
	If IsNothing (kelx) then Throw New ArgumentNullException  ("kelx cannot be Nothing")
	If IsNothing (kely) then Throw New ArgumentNullException  ("kely cannot be Nothing")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixConvolveSep( pixs.Pointer, kelx.Pointer, kely.Pointer, outdepth, normflag)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (2074, 1)
' pixConvolveRGB(pixs, kel) as Pix
' pixConvolveRGB(PIX *, L_KERNEL *) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) This gives a convolution on an RGB image using an<para/>
''' arbitrary kernel (which we normalize to keep each<para/>
''' component within the range [0 ... 255].<para/>
''' (2) The input pixs must be RGB.<para/>
''' (3) The kernel values can be positive or negative, but the<para/>
''' result for the convolution can only be stored as a positive<para/>
''' number.  Consequently, if it goes negative, we clip the<para/>
''' result to 0.<para/>
''' (4) To get a subsampled output, call l_setConvolveSampling().<para/>
''' The time to make a subsampled output is reduced by the<para/>
''' product of the sampling factors.<para/>
''' (5) This uses a mirrored border to avoid special casing on<para/>
''' the boundaries.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 32 bpp rgb</param>
'''  <param name="kel">[in] - kernel</param>
'''   <returns>pixd 32 bpp rgb</returns>
Public Shared Function pixConvolveRGB(
				 ByVal pixs as Pix, 
				 ByVal kel as L_Kernel) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")
	If IsNothing (kel) then Throw New ArgumentNullException  ("kel cannot be Nothing")

	If {32}.contains (pixs.d) = false then Throw New ArgumentException ("32 bpp rgb")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixConvolveRGB( pixs.Pointer, kel.Pointer)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (2133, 1)
' pixConvolveRGBSep(pixs, kelx, kely) as Pix
' pixConvolveRGBSep(PIX *, L_KERNEL *, L_KERNEL *) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) This does a convolution on an RGB image using a separable<para/>
''' kernel that is a sequence of convolutions in x and y.  The two<para/>
''' one-dimensional kernel components must be input separately<para/>
''' the full kernel is the product of these components.<para/>
''' The support for the full kernel is thus a rectangular region.<para/>
''' (2) The kernel values can be positive or negative, but the<para/>
''' result for the convolution can only be stored as a positive<para/>
''' number.  Consequently, if it goes negative, we clip the<para/>
''' result to 0.<para/>
''' (3) To get a subsampled output, call l_setConvolveSampling().<para/>
''' The time to make a subsampled output is reduced by the<para/>
''' product of the sampling factors.<para/>
''' (4) This uses a mirrored border to avoid special casing on<para/>
''' the boundaries.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 32 bpp rgb</param>
'''  <param name="kelx">[in] - x-dependent kernel</param>
'''  <param name="kely">[in] - y-dependent kernel</param>
'''   <returns>pixd 32 bpp rgb</returns>
Public Shared Function pixConvolveRGBSep(
				 ByVal pixs as Pix, 
				 ByVal kelx as L_Kernel, 
				 ByVal kely as L_Kernel) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")
	If IsNothing (kelx) then Throw New ArgumentNullException  ("kelx cannot be Nothing")
	If IsNothing (kely) then Throw New ArgumentNullException  ("kely cannot be Nothing")

	If {32}.contains (pixs.d) = false then Throw New ArgumentException ("32 bpp rgb")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixConvolveRGBSep( pixs.Pointer, kelx.Pointer, kely.Pointer)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (2195, 1)
' fpixConvolve(fpixs, kel, normflag) as FPix
' fpixConvolve(FPIX *, L_KERNEL *, l_int32) as FPIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) This gives a float convolution with an arbitrary kernel.<para/>
''' (2) If normflag == 1, the result is normalized by scaling all<para/>
''' kernel values for a unit sum.  If the sum of kernel values<para/>
''' is very close to zero, the kernel can not be normalized and<para/>
''' the convolution will not be performed.  A warning is issued.<para/>
''' (3) With the FPix, there are no issues about negative<para/>
''' array or kernel values.  The convolution is performed<para/>
''' with single precision arithmetic.<para/>
''' (4) To get a subsampled output, call l_setConvolveSampling().<para/>
''' The time to make a subsampled output is reduced by the<para/>
''' product of the sampling factors.<para/>
''' (5) This uses a mirrored border to avoid special casing on<para/>
''' the boundaries.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="fpixs">[in] - 32 bit float array</param>
'''  <param name="kel">[in] - kernel</param>
'''  <param name="normflag">[in] - 1 to normalize kernel to unit sum 0 otherwise</param>
'''   <returns>fpixd 32 bit float array</returns>
Public Shared Function fpixConvolve(
				 ByVal fpixs as FPix, 
				 ByVal kel as L_Kernel, 
				 ByVal normflag as Integer) as FPix

	If IsNothing (fpixs) then Throw New ArgumentNullException  ("fpixs cannot be Nothing")
	If IsNothing (kel) then Throw New ArgumentNullException  ("kel cannot be Nothing")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.fpixConvolve( fpixs.Pointer, kel.Pointer, normflag)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new FPix(_Result)
End Function

' SRC\convolve.c (2289, 1)
' fpixConvolveSep(fpixs, kelx, kely, normflag) as FPix
' fpixConvolveSep(FPIX *, L_KERNEL *, L_KERNEL *, l_int32) as FPIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) This does a convolution with a separable kernel that is<para/>
''' is a sequence of convolutions in x and y.  The two<para/>
''' one-dimensional kernel components must be input separately<para/>
''' the full kernel is the product of these components.<para/>
''' The support for the full kernel is thus a rectangular region.<para/>
''' (2) The normflag parameter is used as in fpixConvolve().<para/>
''' (3) Warning: if you use l_setConvolveSampling() to get a<para/>
''' subsampled output, and the sampling factor is larger than<para/>
''' the kernel half-width, it is faster to use the non-separable<para/>
''' version pixConvolve().  This is because the first convolution<para/>
''' here must be done on every raster line, regardless of the<para/>
''' vertical sampling factor.  If the sampling factor is smaller<para/>
''' than kernel half-width, it's faster to use the separable<para/>
''' convolution.<para/>
''' (4) This uses mirrored borders to avoid special casing on<para/>
''' the boundaries.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="fpixs">[in] - 32 bit float array</param>
'''  <param name="kelx">[in] - x-dependent kernel</param>
'''  <param name="kely">[in] - y-dependent kernel</param>
'''  <param name="normflag">[in] - 1 to normalize kernel to unit sum 0 otherwise</param>
'''   <returns>fpixd 32 bit float array</returns>
Public Shared Function fpixConvolveSep(
				 ByVal fpixs as FPix, 
				 ByVal kelx as L_Kernel, 
				 ByVal kely as L_Kernel, 
				 ByVal normflag as Integer) as FPix

	If IsNothing (fpixs) then Throw New ArgumentNullException  ("fpixs cannot be Nothing")
	If IsNothing (kelx) then Throw New ArgumentNullException  ("kelx cannot be Nothing")
	If IsNothing (kely) then Throw New ArgumentNullException  ("kely cannot be Nothing")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.fpixConvolveSep( fpixs.Pointer, kelx.Pointer, kely.Pointer, normflag)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new FPix(_Result)
End Function

' SRC\convolve.c (2367, 1)
' pixConvolveWithBias(pixs, kel1, kel2, force8, pbias) as Pix
' pixConvolveWithBias(PIX *, L_KERNEL *, L_KERNEL *, l_int32, l_int32 *) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) This does a convolution with either a single kernel or<para/>
''' a pair of separable kernels, and automatically applies whatever<para/>
''' bias (shift) is required so that the resulting pixel values<para/>
''' are non-negative.<para/>
''' (2) The kernel is always normalized.  If there are no negative<para/>
''' values in the kernel, a standard normalized convolution is<para/>
''' performed, with 8 bpp output.  If the sum of kernel values is<para/>
''' very close to zero, the kernel can not be normalized and<para/>
''' the convolution will not be performed.  An error message results.<para/>
''' (3) If there are negative values in the kernel, the pix is<para/>
''' converted to an fpix, the convolution is done on the fpix, and<para/>
''' a bias (shift) may need to be applied.<para/>
''' (4) If force8 == TRUE and the range of values after the convolution<para/>
''' is  is greater  255, the output values will be scaled to fit in [0 ... 255].<para/>
''' If force8 == FALSE, the output will be either 8 or 16 bpp,<para/>
''' to accommodate the dynamic range of output values without scaling.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 8 bpp no colormap</param>
'''  <param name="kel1">[in] - </param>
'''  <param name="kel2">[in]can be null - use if separable</param>
'''  <param name="force8">[in] - if 1, force output to 8 bpp otherwise, determine output depth by the dynamic range of pixel values</param>
'''  <param name="pbias">[out] - applied bias</param>
'''   <returns>pixd 8 or 16 bpp</returns>
Public Shared Function pixConvolveWithBias(
				 ByVal pixs as Pix, 
				 ByVal kel1 as L_Kernel, 
				 ByVal kel2 as L_Kernel, 
				 ByVal force8 as Integer, 
				<Out()> ByRef pbias as Integer) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")
	If IsNothing (kel1) then Throw New ArgumentNullException  ("kel1 cannot be Nothing")

	Dim kel2PTR As IntPtr = IntPtr.Zero : If Not IsNothing(kel2) Then kel2PTR = kel2.Pointer

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixConvolveWithBias( pixs.Pointer, kel1.Pointer, kel2PTR, force8, pbias)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (2457, 1)
' l_setConvolveSampling(xfact, yfact) as Object
' l_setConvolveSampling(l_int32, l_int32) as void
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) This sets the x and y output subsampling factors for generic pix<para/>
''' and fpix convolution.  The default values are 1 (no subsampling).<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="xfact">[in] - integer  is greater = 1</param>
'''  <param name="yfact">[in] - integer  is greater = 1</param>
Public Shared Sub l_setConvolveSampling(
				 ByVal xfact as Integer, 
				 ByVal yfact as Integer)

	LeptonicaSharp.Natives.l_setConvolveSampling( xfact, yfact)

End Sub

' SRC\convolve.c (2484, 1)
' pixAddGaussianNoise(pixs, stdev) as Pix
' pixAddGaussianNoise(PIX *, l_float32) as PIX *
'''  <summary>
''' <para/>
''' Notes:<para/>
''' (1) This adds noise to each pixel, taken from a normal<para/>
''' distribution with zero mean and specified standard deviation.<para/>
'''  </summary>
'''  <remarks>
'''  </remarks>
'''  <param name="pixs">[in] - 8 bpp gray or 32 bpp rgb no colormap</param>
'''  <param name="stdev">[in] - of noise</param>
'''   <returns>pixd 8 or 32 bpp, or NULL on error</returns>
Public Shared Function pixAddGaussianNoise(
				 ByVal pixs as Pix, 
				 ByVal stdev as Single) as Pix

	If IsNothing (pixs) then Throw New ArgumentNullException  ("pixs cannot be Nothing")

	Dim _Result as IntPtr = LeptonicaSharp.Natives.pixAddGaussianNoise( pixs.Pointer, stdev)
	If  _Result = IntPtr.Zero then Return Nothing

	Return  new Pix(_Result)
End Function

' SRC\convolve.c (2547, 1)
' gaussDistribSampling() as Single
' gaussDistribSampling() as l_float32
'''  <remarks>
'''  </remarks>
'''   <returns></returns>
Public Shared Function gaussDistribSampling() as Single

	Dim _Result as Single = LeptonicaSharp.Natives.gaussDistribSampling( )

	Return _Result
End Function

End Class