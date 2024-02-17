.data

; =====================================================================================
; Author: Mateusz Malek
; Institution: Silesian University of Technology
; Academic Year: 2023/24
; Project: Assembly Project v1.0
; =====================================================================================
;
; Section Overview:
; The data section is organized into three distinct categories, each serving a specific 
; purpose in image processing tasks. These categories include masks for the Laplacian 
; filter, masks for the Gaussian filter, and masks for channel extraction. Each category 
; is designed to aid in various aspects of image enhancement and analysis, from edge 
; detection to blurring and noise reduction, and finally, color channel isolation.
;
; -------------------------------------------------------------------------------------
; 1. Laplacian Filter Masks:
; Used for edge detection by emphasizing areas of rapid intensity change in the image.
;
    minus   dd -0.5, -0.5, -0.5, -0.5   ; Weights for the surrounding pixels in Laplacian filter
    laplace dd 5.0, 5.0, 5.0, 5.0       ; Weight for the center pixel in Laplacian filter
;
; -------------------------------------------------------------------------------------
; 2. Gaussian Filter Masks:
; Employed for blurring and noise reduction, these masks help in reducing detail 
; and noise by averaging pixel values.
;
    corner  dd 1.0, 1.0, 1.0, 1.0       ; Gaussian weight for corner pixels
    edge    dd 2.0, 2.0, 2.0, 2.0       ; Gaussian weight for edge/cross pixels
    center  dd 4.0, 4.0, 4.0, 4.0       ; Gaussian weight for the center pixel
    divide  dd 16.0, 16.0, 16.0, 16.0   ; Division factor for Gaussian filter normalization
;
; -------------------------------------------------------------------------------------
; 3. Channel Extraction:
; These masks are utilized for isolating specific color channels (Red, Green, Blue) 
; from an image, enabling channel-specific processing.
;
    redMask   dd 00FF0000h              ; Mask for extracting the red channel
    greenMask dd 0000FF00h              ; Mask for extracting the green channel
    blueMask  dd 000000FFh              ; Mask for extracting the blue channel
;
; =====================================================================================


.code

; =====================================================================================
;
; Usage of the function:
; The function expects four parameters in the following format:
; (int[] oldArray, 10, 89, int[] newArray)
;
; -------------------------------------------------------------------------------------
;
; Parameters:
; RCX - oldArray:         An array of integers representing pixel data for a 10x10 square. Each integer
;                         is in the format EE-RR-GG-BB, where E = empty (unused bits), R = red, G = green,
;                         B = blue. This array represents the original pixel data before any processing.
;
; RDX - newArray:         An array of integers that will store the modified pixel data after processing.
;                         This array should be initialized with the same size as oldArray.
;
; R8  - startValue (10):  An integer representing the start index for processing, avoiding the
;                         first row and column of the 10x10 square (edges).
;
; R9  - endValue (89):    An integer representing the end index for processing, avoiding the last
;                         row and column of the 10x10 square (edges).
;
; -------------------------------------------------------------------------------------
;
; Description:
; The function processes a 10x10 square of pixel data from oldArray, avoiding the edges
; by starting at index 10 and ending at index 89. This ensures that the outermost pixels
; (edges) are not processed. The modifications are applied to newArray, where the initial
; content is a copy of oldArray. This initial setup ensures that the unmodified edge pixels
; from oldArray are preserved in newArray. The format of pixel data in newArray remains the
; same as in oldArray (EE-RR-GG-BB). The use of newArray allows for the original edge pixels
; to remain unchanged while the inner pixels are processed and modified.
;
; -------------------------------------------------------------------------------------
;
; Initialized values:
; RCX - pointer to first element of the array
; RDX - empty array to fill     
; R8  - start of the array       
; R9  - end of the array
;
; -------------------------------------------------------------------------------------
;
; Returned value:
; The function modifies the newArray, which is passed as a parameter through the RDX register.
; The newArray will contain the modified pixel data after the function execution.
; Procedure returns 0 if completed succesfully
;
; -------------------------------------------------------------------------------------
;
; Registers used:
; RCX - Pointer to the oldArray (input pixel data)
; RDX - Pointer to the newArray (where the modified pixel data will be stored)
; R8  - Start index for processing, used to navigate the processing window within the oldArray
; R9  - End index for processing, used to determine the stop condition for the processing loop
; RSI - Used as a row counter to navigate through the oldArray
; RBX - Temporary register for calculations related to array indexing
; R10, R11, R12 - Temporary registers for channel extraction and processing
; RAX - Used for intermediate storage of pixel data during processing
; XMM0 to XMM3, XMM9 - SSE registers used for processing pixel values (floating-point operations)
;
; =====================================================================================

LaplacianFilterASM proc

    mov RSI, 19                         ; Initialize row counter (end of the second row)

    mov RBX, R8                         ; Copy start index to RBX
    add RBX, RBX
    add RBX, RBX                        ; Multiply start index (RBX) by 4 (to get byte offset)
    add RCX, RBX                        ; Move to section start
    add RDX, RBX                        ; Move to section start

startLoop: 

    xorps xmm0, xmm0                    ; Initializing xmm0 (new pixel value)              

    mov eax, dword ptr [RCX - 36]       ; Left upper edge   [X - -]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - -]
    call processPixel                   ; Call process func [- - -]

    mov eax, dword ptr [RCX - 32]       ; Upper edge        [- X -]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - -]
    call processPixel                   ; Call process func [- - -]

    mov eax, dword ptr [RCX - 28]       ; Right upper edge  [- - X]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - -]
    call processPixel                   ; Call process func [- - -]

    mov eax, dword ptr [RCX - 4]        ; Left edge         [- - -]
    movups xmm9, dword ptr [minus]      ; Load weight       [X - -]
    call processPixel                   ; Call process func [- - -]

    mov eax, dword ptr [RCX]            ; Center            [- - -]
    movups xmm9, dword ptr [laplace]    ; Load weight       [- X -]
    call processPixel                   ; Call process func [- - -]

    mov eax, dword ptr [RCX + 4]        ; Right edge        [- - -]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - X]
    call processPixel                   ; Call process func [- - -]

    mov eax, dword ptr [RCX + 28]       ; Left lower edge   [- - -]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - -]
    call processPixel                   ; Call process func [X - -]

    mov eax, dword ptr [RCX + 32]       ; Lower edge        [- - -]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - -]
    call processPixel                   ; Call process func [- X -]

    mov eax, dword ptr [RCX + 36]       ; Right lower edge  [- - -]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - -]
    call processPixel                   ; Call process func [- - X]

    cvtps2dq xmm0, xmm0                 ; Convert float to integer
    packusdw xmm0, xmm0                 ; Pack values to 16-bit
    packuswb xmm0, xmm0                 ; Pack values to 8-bit
    movd eax, xmm0                      ; Copy xmm0 to eax
    and eax, 00FFFFFFh                  ; Set alpha channel to 0x00
 
    mov [RDX], eax                      ; Set new pixel to empty array

    add RCX, 4                          ; Increment array pointer
    add RDX, 4                          ; Increment array pointer
    inc R8                              ; Increment array index

    cmp R8, RSI                         ; Check if the end of the row
    jl continueLoop                     ; If not end of the row, continue

                                        
    add RCX, 8                          ; Skip last element of the row and the first element of the next row in original array
    add RDX, 8                          ; Increment the new array to align with original one 
    add R8, 2                           ; Increment array index
                                        
    add RSI, 10                         ; Increment row counter to the end of the next row

continueLoop:
    cmp R8, R9                          ; Check if reached the end index
    jl startLoop                        ; If not reached, continue loop
    ret                                 ; If reached, return from procedure

processPixel:
                                        ; Extracting channels and converting from int to float
    mov R10d, eax                       ; Copying pixel to R10
    and R10d, redMask                   ; Extracting red channel
    shr R10d, 16                        ; Moving channel to LSB
    cvtsi2ss xmm1, R10d                 ; Convert red channel to float and load to xmm0

    mov R11d, eax                       ; Copying pixel to R11
    and R11d, greenMask                 ; Extracting green channel
    shr R11d, 8                         ; Moving channel to LSB
    cvtsi2ss xmm2, R11d                 ; Convert green channel to float and load to xmm0

    mov R12d, eax                       ; Copying pixel to R11
    and R12d, blueMask                  ; Extracting blue channel
    cvtsi2ss xmm3, R12d                 ; Convert blue channel to float and load to xmm0

                                        ; Combining channels R, G, B in xmm0
    shufps xmm3, xmm3, 0                ; Duplicate channel B
    shufps xmm2, xmm2, 0                ; Duplicate channel G
    unpcklps xmm3, xmm2                 ; Combine B and G
    movlhps xmm3, xmm1                  ; Add R to xmm0

    mulps xmm3, xmm9                    ; Multiplying by weight
    addps xmm0, xmm3                    ; Adding to xmm0
    ret                                 ; Return to main fuction

LaplacianFilterASM endp

; =====================================================================================
;
; Usage of the function:
; The function expects four parameters in the following format:
; (int[] oldArray, 10, 89, int[] newArray)
;
; -------------------------------------------------------------------------------------
;
; Parameters:
; RCX - oldArray:         An array of integers representing pixel data for a 10x10 square. Each integer
;                         is in the format EE-RR-GG-BB, where E = empty (unused bits), R = red, G = green,
;                         B = blue. This array represents the original pixel data before any processing.
;
; RDX - newArray:         An array of integers that will store the modified pixel data after processing.
;                         This array should be initialized with the same size as oldArray.
;
; R8  - startValue (10):  An integer representing the start index for processing, avoiding the
;                         first row and column of the 10x10 square (edges).
;
; R9  - endValue (89):    An integer representing the end index for processing, avoiding the last
;                         row and column of the 10x10 square (edges).
;
; -------------------------------------------------------------------------------------
;
; Description:
; The function processes a 10x10 square of pixel data from oldArray, avoiding the edges
; by starting at index 10 and ending at index 89. This ensures that the outermost pixels
; (edges) are not processed. The modifications are applied to newArray, where the initial
; content is a copy of oldArray. This initial setup ensures that the unmodified edge pixels
; from oldArray are preserved in newArray. The format of pixel data in newArray remains the
; same as in oldArray (EE-RR-GG-BB). The use of newArray allows for the original edge pixels
; to remain unchanged while the inner pixels are processed and modified.
;
; -------------------------------------------------------------------------------------
;
; Initialized values:
; RCX - pointer to first element of the array
; RDX - empty array to fill     
; R8  - start of the array       
; R9  - end of the array
;
; -------------------------------------------------------------------------------------
;
; Returned value:
; The function modifies the newArray, which is passed as a parameter through the RDX register.
; The newArray will contain the modified pixel data after the function execution.
; Procedure returns 0 if completed succesfully
;
; -------------------------------------------------------------------------------------
;
; Registers used:
; RCX - Pointer to the oldArray (input pixel data)
; RDX - Pointer to the newArray (where the modified pixel data will be stored)
; R8  - Start index for processing, used to navigate the processing window within the oldArray
; R9  - End index for processing, used to determine the stop condition for the processing loop
; RSI - Used as a row counter to navigate through the oldArray
; RBX - Temporary register for calculations related to array indexing
; R10, R11, R12 - Temporary registers for channel extraction and processing
; RAX - Used for intermediate storage of pixel data during processing
; XMM0 to XMM3, XMM9 - SSE registers used for processing pixel values (floating-point operations)
;
; =====================================================================================

GaussianBlurASM proc

    mov RSI, 19                         ; Initialize row counter (end of the second row)

    mov RBX, R8                         ; Copy start index to RBX
    add RBX, RBX
    add RBX, RBX                        ; Multiply start index (RBX) by 4 (to get byte offset)
    add RCX, RBX                        ; Move to section start
    add RDX, RBX                        ; Move to section start

startLoop: 

    xorps xmm0, xmm0                    ; Initializing xmm0 (new pixel value)              

    mov eax, dword ptr [RCX - 36]       ; Left upper edge   [X - -]
    movups xmm9, dword ptr [corner]     ; Load weight       [- - -]
    call processPixel                   ; Call process func [- - -]

    mov eax, dword ptr [RCX - 32]       ; Upper edge        [- X -]
    movups xmm9, dword ptr [edge]       ; Load weight       [- - -]
    call processPixel                   ; Call process func [- - -]

    mov eax, dword ptr [RCX - 28]       ; Right upper edge  [- - X]
    movups xmm9, dword ptr [corner]     ; Load weight       [- - -]
    call processPixel                   ; Call process func [- - -]

    mov eax, dword ptr [RCX - 4]        ; Left edge         [- - -]
    movups xmm9, dword ptr [edge]       ; Load weight       [X - -]
    call processPixel                   ; Call process func [- - -]

    mov eax, dword ptr [RCX]            ; Center            [- - -]
    movups xmm9, dword ptr [center]     ; Load weight       [- X -]
    call processPixel                   ; Call process func [- - -]

    mov eax, dword ptr [RCX + 4]        ; Right edge        [- - -]
    movups xmm9, dword ptr [edge]       ; Load weight       [- - X]
    call processPixel                   ; Call process func [- - -]

    mov eax, dword ptr [RCX + 28]       ; Left lower edge   [- - -]
    movups xmm9, dword ptr [corner]     ; Load weight       [- - -]
    call processPixel                   ; Call process func [X - -]

    mov eax, dword ptr [RCX + 32]       ; Lower edge        [- - -]
    movups xmm9, dword ptr [edge]       ; Load weight       [- - -]
    call processPixel                   ; Call process func [- X -]

    mov eax, dword ptr [RCX + 36]       ; Right lower edge  [- - -]
    movups xmm9, dword ptr [corner]     ; Load weight       [- - -]
    call processPixel                   ; Call process func [- - X]

    divps xmm0, divide                  ; Divide sum of the channels by total weight to get new values of the pixel

    cvtps2dq xmm0, xmm0                 ; Convert float to integer
    packusdw xmm0, xmm0                 ; Pack values to 16-bit
    packuswb xmm0, xmm0                 ; Pack values to 8-bit
    movd eax, xmm0                      ; Copy xmm0 to eax
    and eax, 00FFFFFFh                  ; Set alpha channel to 0x00
 
    mov [RDX], eax                      ; Set new pixel to empty array

    add RCX, 4                          ; Increment array pointer
    add RDX, 4                          ; Increment array pointer
    inc R8                              ; Increment array index

    cmp R8, RSI                         ; Check if the end of the row
    jl continueLoop                     ; If not end of the row, continue
                                     
    add RCX, 8                          ; Skip last element of the row and the first element of the next row in original array
    add RDX, 8                          ; Increment the new array to align with original one 
    add R8, 2                           ; Increment array index
                                       
    add RSI, 10                         ; Increment row counter to the end of the next row

continueLoop:
    cmp R8, R9                          ; Check if reached the end index
    jl startLoop                        ; If not reached, continue loop
    ret                                 ; If reached, return from procedure

processPixel:
                                        ; Extracting channels and converting from int to float
    mov R10d, eax                       ; Copying pixel to R10
    and R10d, redMask                   ; Extracting red channel
    shr R10d, 16                        ; Moving channel to LSB
    cvtsi2ss xmm1, R10d                 ; Convert red channel to float and load to xmm0

    mov R11d, eax                       ; Copying pixel to R11
    and R11d, greenMask                 ; Extracting green channel
    shr R11d, 8                         ; Moving channel to LSB
    cvtsi2ss xmm2, R11d                 ; Convert green channel to float and load to xmm0

    mov R12d, eax                       ; Copying pixel to R11
    and R12d, blueMask                  ; Extracting blue channel
    cvtsi2ss xmm3, R12d                 ; Convert blue channel to float and load to xmm0

                                        ; Combining channels R, G, B in xmm0
    shufps xmm3, xmm3, 0                ; Duplicate channel B
    shufps xmm2, xmm2, 0                ; Duplicate channel G
    unpcklps xmm3, xmm2                 ; Combine B and G
    movlhps xmm3, xmm1                  ; Add R to xmm0

    mulps xmm3, xmm9                    ; Multiplying by weight
    addps xmm0, xmm3                    ; Adding to xmm0
    ret                                 ; Return to main function

GaussianBlurASM endp

end

