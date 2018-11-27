'Window Shop 2
'1.0


'This script searches for container names
'Do not modify names


'This scripts assumes that only items with item number will be used, ignotes inputs without item numbers
'This script assumes that each item will have atleast one image

'Arrays for tracking populated item groups & Images
'Current counts within the arrays
'Boolean for which timeline to run
Dim Items As Array[Integer]
Dim Images as Array[Integer]
Dim Texts as Array[Integer]
Dim ItemCount As Integer
Dim ImageCount As Integer
Dim TextCount As Integer
Dim NewItem As Boolean
Dim NewImage As Boolean

'Used to load info on the different page looks
'SetText, SetImage, Copy2to1, CopyBto1
Dim PageLooks As Array[String]
PageLooks.Push("FullOutput")
PageLooks.Push("SideOutput")

'Input values on the script container
'Add more input groups in the tree if needed
'Modify StartTimers() if more than 5 Images will be needed
Sub OnInitParameters()
	RegisterParameterString("NumberOfItems","Number of Input Groups","5",50,256,"")
	RegisterParameterString("NumberOfImages","Number of Input Images","3",50,256,"")
	RegisterParameterString("OneImageTime","1 Image Time","10000",50,256,"")
    RegisterParameterString("TwoImageTime","2 Image Time for Each","5000",50,256,"")
    RegisterParameterString("ThreeImageTime","3 Image Time for Each","3333",50,256,"")
    RegisterParameterString("FourImageTime","4 Image Time for Each","2500",50,256,"")
    RegisterParameterString("FiveImageTime","5 Image Time for Each","2000",50,256,"")
End Sub

Sub OnInit()
    this.geometry.RegisterTextChangedCallback
    'Get Input Values & Preview First Filled In Input Group
    GetInputs()
    PreviewItem(CStr(Items[0]))
End Sub

Sub OnGeometryChanged(geom As Geometry)
     'Preview whichever input group changed
     PreviewItem(this.geometry.Text)
End Sub

Sub PreviewItem(Inputs As String)
    StopTimers()
     Dim ImageNumber As Integer
     'Look to see if image is indicated or find first filled in image	 
     If Inputs.find("|") > -1 then
          Dim ItemImage as Array[String]
          Inputs.split("|",ItemImage)
          Inputs = ItemImage[0]
		  ImageNumber = CInt(ItemImage[1])
     Else
        Images.Clear
        For i = 1 to  CInt(GetParameterString("NumberOfImages"))
            If GetText(CInt(Inputs), "Image" & CStr(i)) <> "" then Images.Push(i)
        Next	
		
        ImageNumber = Images[0]
     End If
	 
    Texts.Clear
    For i = 1 to  CInt(GetParameterString("NumberOfTexts"))
        If GetText(CInt(Inputs), "Text" & CStr(i)) <> "" then Texts.Push(i)
    Next	
		
        ImageNumber = Images[0]
	 
    stage.finddirector("Default").show(3.0)	 		 
    stage.finddirector("Item_Trans").startanimation()
    stage.finddirector("Image_Trans").startanimation()
    stage.finddirector("Text_Trans").startanimation()
	
     SetItem(Cint(Inputs), ImageNumber, 1)
End Sub

Sub GetInputs()
     'Clear Arrays
     Items.Clear

     'Find which items have a populated Item Number to use for the loop
     For i = 1 to CInt(GetParameterString("NumberOfItems"))
          If GetText(i, "Item_Number") <> "" then Items.Push(i)
     Next
	 
End Sub

Sub Load()
    'Get what itmes are loaded & set the first item
    'Reset counters
    'Setup first transiton (Item or Image)
    GetInputs()
    PreviewItem(CStr(Items[0]))
	
    ItemCount = 1
	ImageCount = 1
    TextCount = 1
	
    TransitionType()
End Sub

Sub StopTimers()
    Dim Container As String
	
    Container = CStr(Scene.FindContainer("Item_Timer"))
    System.SendCommand("0 #" & Container & "*FUNCTION*DataTimer*enabled SET 0;")
    Container = CStr(Scene.FindContainer("Image_Timer"))
    System.SendCommand("0 #" & Container & "*FUNCTION*DataTimer*enabled SET 0;")
    Container = CStr(Scene.FindContainer("Text_Timer"))
    System.SendCommand("0 #" & Container & "*FUNCTION*DataTimer*enabled SET 0;")			
End Sub

'Based on TransitionType() sets either a change in item image or sets the timer to transition to a new item
Sub StartTimers()
    Dim Container As String
    Dim TimerTime As Integer
	
    If NewImage = True Then
        Container = CStr(Scene.FindContainer("Image_Timer"))
        If Images.UBound + 1 = 2 then TimerTime = CInt(GetParameterString("TwoImageTime"))
        If Images.UBound + 1 = 3 then TimerTime = CInt(GetParameterString("ThreeImageTime"))
        If Images.UBound + 1 = 4 then TimerTime = CInt(GetParameterString("FourImageTime"))
        If Images.UBound + 1 = 5 then TimerTime = CInt(GetParameterString("FiveImageTime"))
		
        System.SendCommand("0 #" & Container & "*FUNCTION*DataTimer*interval SET " & CStr(TimerTime) & ";")
        System.SendCommand("0 #" & Container & "*FUNCTION*DataTimer*enabled SET 1;")
    Else
	    If NewItem = True Then
            Container = CStr(Scene.FindContainer("Item_Timer"))
            If Images.UBound + 1 = 1 then TimerTime = CInt(GetParameterString("OneImageTime"))
            If Images.UBound + 1 = 2 then TimerTime = CInt(GetParameterString("TwoImageTime"))
            If Images.UBound + 1 = 3 then TimerTime = CInt(GetParameterString("ThreeImageTime"))
            If Images.UBound + 1 = 4 then TimerTime = CInt(GetParameterString("FourImageTime"))
            If Images.UBound + 1 = 5 then TimerTime = CInt(GetParameterString("FiveImageTime"))
			
            System.SendCommand("0 #" & Container & "*FUNCTION*DataTimer*interval SET " & CStr(TimerTime) & ";")
	        System.SendCommand("0 #" & Container & "*FUNCTION*DataTimer*enabled SET 1;")		
        End If	
    End If
	
    Container = CStr(Scene.FindContainer("Text_Timer"))	
    If Images.UBound + 1 = 1 then TimerTime = CInt(GetParameterString("OneImageTime"))
    If Images.UBound + 1 = 2 then TimerTime = CInt(GetParameterString("TwoImageTime"))
    If Images.UBound + 1 = 3 then TimerTime = CInt(GetParameterString("ThreeImageTime"))
    If Images.UBound + 1 = 4 then TimerTime = CInt(GetParameterString("FourImageTime"))
    If Images.UBound + 1 = 5 then TimerTime = CInt(GetParameterString("FiveImageTime"))

    If Texts.UBound + 1 > 1 Then
	    TimerTime = (TimerTime/(Texts.UBound + 1))
        System.SendCommand("0 #" & Container & "*FUNCTION*DataTimer*interval SET " & CStr(TimerTime) & ";")	
        System.SendCommand("0 #" & Container & "*FUNCTION*DataTimer*enabled SET 1;")
    End If
End Sub

'stops timers
'Incriment counters
'Set up next image and item, loops to the start if at the end of the array
'Only loops images if one item
'no tranitions if one item and one image
'sets timers based on current itemm
Sub Loop()
	StopTimers()
	
    Dim ImagePath As String
    Dim PosX As Double
    Dim PosY As Double
    Dim ScaleX As Double
    Dim ScaleY As Double	
	

   Images.Clear
    For i = 1 to GetParameterString("NumberOfImages")
        If GetText(Items[ItemCount - 1], "Image" & CStr(i)) <> "" then Images.Push(i)
    Next

    Texts.Clear
    For i = 1 to  CInt(GetParameterString("NumberOfTexts"))
        If GetText(CInt(Inputs), "Text" & CStr(i)) <> "" then Texts.Push(i)
    Next	
	
    'Increment Images
    ImageCount = ImageCount + 1	
    If ImageCount > Images.UBound + 1 then
        ImageCount = 1
		
        TextCount = 1
		
        'Increment Items
        ItemCount = ItemCount + 1
        If ItemCount > Items.UBound + 1 then ItemCount = 1					
    End If
	
	
	
   Images.Clear
    For i = 1 to GetParameterString("NumberOfImages")
        If GetText(Items[ItemCount - 1], "Image" & CStr(i)) <> "" then Images.Push(i)
    Next
	
    Texts.Clear
    For i = 1 to  CInt(GetParameterString("NumberOfTexts"))
        If GetText(CInt(Inputs), "Text" & CStr(i)) <> "" then Texts.Push(i)
    Next	
	
    TransitionType()
	
    'Update B image
    ImagePath = GetText(Items[ItemCount - 1], "Image" & CStr(Images[ImageCount - 1]))
    PosX = GetImagePosX(Items[ItemCount - 1], Images[ImageCount - 1])
    PosY = GetImagePosY(Items[ItemCount - 1], Images[ImageCount - 1])
    ScaleX = GetImageScaleX(Items[ItemCount - 1], Images[ImageCount - 1])
    ScaleY = GetImageScaleY(Items[ItemCount - 1], Images[ImageCount - 1])
    SetImage(1, "B", ImagePath, PosX, PosY, ScaleX, ScaleY)

    'Update Item 2
    SetItem(Items[ItemCount - 1], Images[ImageCount - 1], 2)
	
	
End Sub

Sub TransitionType()
	'NewItem = True	
    'NewImage = True	
		
    If ImageCount < Images.UBound + 1 Then
	    NewItem = False	
        NewImage = True
    Else
        If Items.UBound + 1 > 1 Then
	        NewItem = True	
            NewImage = False
        ElseIf Images.UBound + 1 > 1 Then
	        NewItem = False	
            NewImage = True
        End If		
    End If

    StartTimers()	
	
	Scene.FindContainer("ItemArray").geometry.text = CStr("Image "& ImageCount & " " & Images.UBound + 1 & " " & NewImage)
	Scene.FindContainer("ImageArray").geometry.text = CStr("Item " & ItemCount & " " & Items.UBound + 1 & " " & NewItem)		

End Sub

'After the item transiions to the 2 output all infor from output 2 is copied back to 1 so the timeline can loop back to 1 and reset for the next item
'This includes text, images, image scale and position
'The info is copied for both original and full screen versions
Sub Copy2to1()
    For i = 0 to PageLooks.UBound
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image_Path").geometry.text = Scene.FindContainer(PageLooks[i]).FindSubContainer("2").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image_Path").geometry.text
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image").position.x = Scene.FindContainer(PageLooks[i]).FindSubContainer("2").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image").position.x
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image").position.y = Scene.FindContainer(PageLooks[i]).FindSubContainer("2").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image").position.y
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image").scaling.x = Scene.FindContainer(PageLooks[i]).FindSubContainer("2").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image").scaling.x
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image").scaling.y = Scene.FindContainer(PageLooks[i]).FindSubContainer("2").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image").scaling.y
	 
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Text").FindSubContainer("Item_Number").geometry.text = Scene.FindContainer(PageLooks[i]).FindSubContainer("2").FindSubContainer("Text").FindSubContainer("Item_Number").geometry.text
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Text").FindSubContainer("Topline").geometry.text = Scene.FindContainer(PageLooks[i]).FindSubContainer("2").FindSubContainer("Text").FindSubContainer("Topline").geometry.text
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Text").FindSubContainer("Percent_Off").geometry.text = Scene.FindContainer(PageLooks[i]).FindSubContainer("2").FindSubContainer("Text").FindSubContainer("Percent_Off").geometry.text
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Text").FindSubContainer("WasText").geometry.text = Scene.FindContainer(PageLooks[i]).FindSubContainer("2").FindSubContainer("Text").FindSubContainer("WasText").geometry.text
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Text").FindSubContainer("Was").geometry.text = Scene.FindContainer(PageLooks[i]).FindSubContainer("2").FindSubContainer("Text").FindSubContainer("Was").geometry.text
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Text").FindSubContainer("Price").geometry.text = Scene.FindContainer(PageLooks[i]).FindSubContainer("2").FindSubContainer("Text").FindSubContainer("Price").geometry.text
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Text").FindSubContainer("Qualifier").geometry.text = Scene.FindContainer(PageLooks[i]).FindSubContainer("2").FindSubContainer("Text").FindSubContainer("Qualifier").geometry.text
    Next
	
    stage.finddirector("Item_Trans").startanimation()
End Sub

'Similar to 2to1, this is used to copy the image only for image transitions
'Output 1 has a main and B image for it's transition
'Loops back to 1 after copying values from Image B
Sub CopyBto1()
    For i = 0 to PageLooks.UBound
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image_Path").geometry.text = Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("ImageB").FindSubContainer("Image_Path").geometry.text
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image").position.x = Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("ImageB").FindSubContainer("Image").position.x
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image").position.y = Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("ImageB").FindSubContainer("Image").position.y
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image").scaling.x = Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("ImageB").FindSubContainer("Image").scaling.x
        Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("Image").FindSubContainer("Image").scaling.y = Scene.FindContainer(PageLooks[i]).FindSubContainer("1").FindSubContainer("Image").FindSubContainer("ImageB").FindSubContainer("Image").scaling.y
    Next
	
    stage.finddirector("Image_Trans").startanimation()
End Sub


'Used to set up the next item & for item preview
'In addition to main text and image this also calculates percent off
'Will display percent off if more than 20%
'Will hide was price if 5% or less
'If there is a price range the higher percent is used
Sub SetItem(Inputs As Integer, ImageNumber As Integer, Item As Integer)
    'Get Image path and position/scale
    Dim ImagePath As String
    Dim PosX As Double
    Dim PosY As Double
    Dim ScaleX As Double
    Dim ScaleY As Double	 
    ImagePath = GetText(Inputs, "Image" & CStr(ImageNumber))
    PosX = GetImagePosX(Inputs, ImageNumber)
    PosY = GetImagePosY(Inputs, ImageNumber)
    ScaleX = GetImageScaleX(Inputs, ImageNumber)
    ScaleY = GetImageScaleY(Inputs, ImageNumber)
	 
    'Update scene image
    SetImage(Item, "",ImagePath, PosX, PosY, ScaleX, ScaleY)
	 
    SetText(Item, "Item_Number", GetText(Inputs, "Item_Number"))
    SetText(Item, "Topline", GetText(Inputs, "Topline"))

    'Calc percent off, If under 20% put the price in the percent field and hide was	 
    Dim Was As String
    Dim Price As String
     Was = GetText(Inputs, "Was")
     Price = GetText(Inputs, "Price")	

    Dim Percent As String
	
    Dim PriceLeft As String
    Dim PriceRight As String	
	
    Dim PercentLeft As String
    Dim PercentRight As String
	 
	
    If Was.find("-") > -1 or Was.find("/") > -1 or Price.find("-") > -1 or Price.find("/") > -1 Then
		
        Dim WasLeft As String
        Dim WasRight As String

        If Was.find("-") > -1 or Was.find("/") > -1 Then
            Dim WasSplit as Array[String]
            If Was.find("-") > -1 then Was.split("-", WasSplit)
            If Was.find("/") > -1 then Was.split("/", WasSplit)
            WasLeft = WasSplit[0]
            WasRight = WasSplit[1]
        Else
            WasLeft = Was
            WasRight = Was
        End If		
	
		
        If Price.find("-") > -1 or Price.find("/") > -1 Then
            Dim PriceSplit as Array[String]
            If Price.find("-") > -1 then Price.split("-", PriceSplit)
            If Price.find("/") > -1 then Price.split("/", PriceSplit)
            PriceLeft = PriceSplit[0]
            PriceRight = PriceSplit[1]
        Else
            PriceLeft = Price
            PriceRight = Price
        End If	
		
        WasLeft = CStr(ToPennies(WasLeft))
        WasRight = CStr(ToPennies(WasRight))
		
        PriceLeft = CStr(ToPennies(PriceLeft))
        PriceRight = CStr(ToPennies(PriceRight))
	
				
        PercentLeft = CStr((CInt(WasLeft) - CInt(PriceLeft)))
        PercentLeft = CStr(CDbl(PercentLeft) / CDbl(WasLeft) * 100)
	    PercentLeft = CStr(CInt(PercentLeft))		

        PercentRight = CStr((CInt(WasRight) - CInt(PriceRight)))
        PercentRight = CStr(CDbl(PercentRight) / CDbl(WasRight) * 100)
	    PercentRight = CStr(CInt(PercentRight))			
		

        If CInt(PercentLeft) > CInt(PercentRight) Then
		    Percent = PercentLeft
        Else
		    Percent = PercentRight		
        End If		
		

    Else
        Was = CStr(ToPennies(Was))
        Price = CStr(ToPennies(Price))
		
        Percent = CStr((CInt(Was) - CInt(Price)))
        Percent = CStr(CDbl(Percent) / CDbl(Was) * 100)
	    Percent = CStr(CInt(Percent))
    End If

    Scene.FindContainer("Percents").geometry.text = PercentLeft & "    " & PercentRight
	 
    If CInt(Percent) > 20 and CInt(Percent) < 100 then
         SetText(Item, "Percent_Off", CStr(Percent & "% OFF"))
         SetText(Item, "WasText", "Was ")
         SetText(Item, "Was", GetText(Inputs, "Was"))
         SetText(Item, "Price", GetText(Inputs, "Price"))
    Else
        If CInt(Percent) > 5 Then
            SetText(Item, "Percent_Off", "")
            SetText(Item, "WasText", "Was ")
            SetText(Item, "Was", GetText(Inputs, "Was"))
            SetText(Item, "Price", GetText(Inputs, "Price"))
        Else
            SetText(Item, "Percent_Off", "")
            SetText(Item, "WasText", "")
            SetText(Item, "Was", "")
            SetText(Item, "Price", GetText(Inputs, "Price"))		
        End If
    End If
	 
    SetText(Item, "Qualifier", GetText(Inputs, "Qualifier"))	 
	 
End Sub



'Remove $ and convert to pennies for calculations
Function ToPennies(Amount as String) as Integer
    Amount.Trim
    Amount.Substitute(",", "", True)
	
     If Amount.find("$") > -1 then
          Dim AmountSplit as Array[String]
          Amount.split("$",AmountSplit)

          If AmountSplit[0] <> "" then
               Amount = AmountSplit[0]
          Else
               Amount = AmountSplit[1]
          End If
     End If

     Amount.Trim
     If Amount.find(".") > -1 then
          Dim CentsSplit as Array[String]
          Amount.split(".",CentsSplit)
          If CentsSplit[1].Length = 1 then CentsSplit[1] = CentsSplit[1] & "0"
          Amount = CStr( (CInt(CentsSplit[0]) * 100) + CInt(CentsSplit[1]))
     Else
           Amount = CStr(CInt(Amount) * 100)
     End if

	 ToPennies = CInt(Amount)
End Function



Function GetText(Item As Integer, Field As String) As String
     GetText = Scene.FindContainer("Input").FindSubContainer(CStr(Item)).FindSubContainer(Field).geometry.text
End Function

Function GetImagePosX(Item As Integer, Image As Integer) As Double
     GetImagePosX = Scene.FindContainer("Input").FindSubContainer(CStr(Item)).FindSubContainer("Image" & CStr(Image)).position.x
End Function

Function GetImagePosY(Item As Integer, Image As Integer) As Double
     GetImagePosY = Scene.FindContainer("Input").FindSubContainer(CStr(Item)).FindSubContainer("Image" & CStr(Image)).position.y
End Function

Function GetImageScaleX(Item As Integer, Image As Integer) As Double
     GetImageScaleX = Scene.FindContainer("Input").FindSubContainer(CStr(Item)).FindSubContainer("Image" & CStr(Image)).scaling.x
End Function

Function GetImageScaleY(Item As Integer, Image As Integer) As Double
     GetImageScaleY = Scene.FindContainer("Input").FindSubContainer(CStr(Item)).FindSubContainer("Image" & CStr(Image)).scaling.y
End Function

Sub SetText(Item As Integer, Field As String, Value As String)
    For i = 0 to PageLooks.UBound
        Scene.FindContainer(PageLooks[i]).FindSubContainer(CStr(Item)).FindSubContainer("Text").FindSubContainer(Field).geometry.text = Value
    Next
End Sub

Sub SetImage(Item As Integer, ImageB As String, ImagePath As String, PosX As Double, PosY As Double, ScaleX As Double, ScaleY As Double)
    For i = 0 to PageLooks.UBound
        Scene.FindContainer(PageLooks[i]).FindSubContainer(CStr(Item)).FindSubContainer("Image").FindSubContainer("Image" & ImageB).FindSubContainer("Image_Path").geometry.text = ImagePath

        Scene.FindContainer(PageLooks[i]).FindSubContainer(CStr(Item)).FindSubContainer("Image").FindSubContainer("Image" & ImageB).FindSubContainer("Image").position.x = PosX
        Scene.FindContainer(PageLooks[i]).FindSubContainer(CStr(Item)).FindSubContainer("Image").FindSubContainer("Image" & ImageB).FindSubContainer("Image").position.y = PosY
        Scene.FindContainer(PageLooks[i]).FindSubContainer(CStr(Item)).FindSubContainer("Image").FindSubContainer("Image" & ImageB).FindSubContainer("Image").scaling.x = ScaleX
        Scene.FindContainer(PageLooks[i]).FindSubContainer(CStr(Item)).FindSubContainer("Image").FindSubContainer("Image" & ImageB).FindSubContainer("Image").scaling.y = ScaleY
    Next
End Sub


