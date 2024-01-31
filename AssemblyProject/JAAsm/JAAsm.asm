.data

; Author: Mateusz Malek
; Silesian University of Technology 2023/24
; Assembly Project v1.0

; Laplacian Filter and Gaussian Blur implementation
; The function takes a pixel and its 8 surrounding pixels, and then uses XMM registers 
; to create a new pixel value by summing the RGB values through a weighted average
; This function is avoiding the edges

    corner  dd 1.0, 1.0, 1.0, 1.0       ; Weight of the corner (gauss)
    diag    dd 2.0, 2.0, 2.0, 2.0       ; Weight of the diagonal (gauss)
    center  dd 4.0, 4.0, 4.0, 4.0       ; Weight of the center (gauss)
    divide  dd 16.0, 16.0, 16.0, 16.0   ; Weight of the division (gauss)
    minus   dd -0.5, -0.5, -0.5, -0.5   ; Weight of the surrounding pixels (laplace)
    laplace dd 5.0, 5.0, 5.0, 5.0       ; Weight of the center (laplace)

    redMask   dd 00FF0000h              ; Value of the red channel
    greenMask dd 0000FF00h              ; Value of the green channel
    blueMask  dd 000000FFh              ; Value of the blue channel

.code

GaussianBlur proc
                                        ; RCX - pointer to first element of the array
                                        ; RDX - start of the array
                                        ; R8  - end of the array
                                        ; R9  - empty array to fill
                                        ; RSI - row counter

    mov RSI, 19                         ; Initialize row counter (end of the second row)

    mov RBX, RDX                        ; Copy start index to R8
    add RBX, RBX
    add RBX, RBX                        ; Multiply start index (R8) by 4 (to get byte offset)
    add RCX, RBX                        ; Move to section start
    add R9, RBX                         ; Move to section start

startLoop: 

    xorps xmm0, xmm0                    ; initializing xmm0 (new pixel value)              

    mov eax, dword ptr [RCX - 36]       ; Left upper edge   [X - -]
    movups xmm9, dword ptr [corner]     ; Load weight       [- - -]
    call processPixel                   ;                   [- - -]

    mov eax, dword ptr [RCX - 32]       ; Upper edge        [- X -]
    movups xmm9, dword ptr [diag]       ; Load weight       [- - -]
    call processPixel                   ;                   [- - -]

    mov eax, dword ptr [RCX - 28]       ; Right upper edge  [- - X]
    movups xmm9, dword ptr [corner]     ; Load weight       [- - -]
    call processPixel                   ;                   [- - -]

    mov eax, dword ptr [RCX - 4]        ; Left edge         [- - -]
    movups xmm9, dword ptr [diag]       ; Load weight       [X - -]
    call processPixel                   ;                   [- - -]

    mov eax, dword ptr [RCX]            ; Center            [- - -]
    movups xmm9, dword ptr [center]     ; Load weight       [- X -]
    call processPixel                   ;                   [- - -]

    mov eax, dword ptr [RCX + 4]        ; Right edge        [- - -]
    movups xmm9, dword ptr [diag]       ; Load weight       [- - X]
    call processPixel                   ;                   [- - -]

    mov eax, dword ptr [RCX + 28]       ; Left lower edge   [- - -]
    movups xmm9, dword ptr [corner]     ; Load weight       [- - -]
    call processPixel                   ;                   [X - -]

    mov eax, dword ptr [RCX + 32]       ; Lower edge        [- - -]
    movups xmm9, dword ptr [diag]       ; Load weight       [- - -]
    call processPixel                   ;                   [- X -]

    mov eax, dword ptr [RCX + 36]       ; Right lower edge  [- - -]
    movups xmm9, dword ptr [corner]     ; Load weight       [- - -]
    call processPixel                   ;                   [- - X]

    divps xmm0, divide 

    cvtps2dq xmm0, xmm0                 ; Convert float to integer
    packusdw xmm0, xmm0                 ; Pack values to 16-bit
    packuswb xmm0, xmm0                 ; Pack values to 8-bit
    movd eax, xmm0                      ; Copy xmm0 to eax
    and eax, 00FFFFFFh                  ; Set alpha channel to 0x00
 
    mov [R9], eax                       ; Set new pixel to empty array

    add RCX, 4                          ;Increment array pointer
    add R9, 4                           ;Increment array pointer
    inc RDX                             ;Increment array index

    cmp RDX, RSI                        ; Check if the end of the row
    jl continueLoop                     ; If not end of the row, continue

                                        ; Skip last element of the row and the first element of the next row
    add RCX, 8
    add R9, 8
    add RDX, 2

                                        ; Increment row counter to the end of the next row
    add RSI, 10

continueLoop:
    cmp RDX, R8                         ; Check if reached the end index
    jl startLoop                        ; If not reached, continue loop
    ret

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
    movlhps xmm3, xmm1                  ; Add B to xmm0

    mulps xmm3, xmm9                    ; Multiplying by weight
    addps xmm0, xmm3                    ; Adding to xmm0
    ret

GaussianBlur endp

LaplacianFilter proc
                                        ; RCX - pointer to first element of the array
                                        ; RDX - start of the array
                                        ; R8  - end of the array
                                        ; R9  - empty array to fill
                                        ; RSI - row counter

    mov RSI, 19                         ; Initialize row counter (end of the second row)

    mov RBX, RDX                        ; Copy start index to R8
    add RBX, RBX
    add RBX, RBX                        ; Multiply start index (R8) by 4 (to get byte offset)
    add RCX, RBX                        ; Move to section start
    add R9, RBX                         ; Move to section start

startLoop: 

    xorps xmm0, xmm0                    ; initializing xmm0 (new pixel value)              

    mov eax, dword ptr [RCX - 36]       ; Left upper edge   [X - -]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - -]
    call processPixel                   ;                   [- - -]

    mov eax, dword ptr [RCX - 32]       ; Upper edge        [- X -]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - -]
    call processPixel                   ;                   [- - -]

    mov eax, dword ptr [RCX - 28]       ; Right upper edge  [- - X]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - -]
    call processPixel                   ;                   [- - -]

    mov eax, dword ptr [RCX - 4]        ; Left edge         [- - -]
    movups xmm9, dword ptr [minus]      ; Load weight       [X - -]
    call processPixel                   ;                   [- - -]

    mov eax, dword ptr [RCX]            ; Center            [- - -]
    movups xmm9, dword ptr [laplace]    ; Load weight       [- X -]
    call processPixel                   ;                   [- - -]

    mov eax, dword ptr [RCX + 4]        ; Right edge        [- - -]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - X]
    call processPixel                   ;                   [- - -]

    mov eax, dword ptr [RCX + 28]       ; Left lower edge   [- - -]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - -]
    call processPixel                   ;                   [X - -]

    mov eax, dword ptr [RCX + 32]       ; Lower edge        [- - -]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - -]
    call processPixel                   ;                   [- X -]

    mov eax, dword ptr [RCX + 36]       ; Right lower edge  [- - -]
    movups xmm9, dword ptr [minus]      ; Load weight       [- - -]
    call processPixel                   ;                   [- - X]

    cvtps2dq xmm0, xmm0                 ; Convert float to integer
    packusdw xmm0, xmm0                 ; Pack values to 16-bit
    packuswb xmm0, xmm0                 ; Pack values to 8-bit
    movd eax, xmm0                      ; Copy xmm0 to eax
    and eax, 00FFFFFFh                  ; Set alpha channel to 0x00
 
    mov [R9], eax                       ; Set new pixel to empty array

    add RCX, 4                          ;Increment array pointer
    add R9, 4                           ;Increment array pointer
    inc RDX                             ;Increment array index

    cmp RDX, RSI                        ; Check if the end of the row
    jl continueLoop                     ; If not end of the row, continue

                                        ; Skip last element of the row and the first element of the next row
    add RCX, 8
    add R9, 8
    add RDX, 2

                                        ; Increment row counter to the end of the next row
    add RSI, 10

continueLoop:
    cmp RDX, R8                         ; Check if reached the end index
    jl startLoop                        ; If not reached, continue loop
    ret

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
    ret

LaplacianFilter endp

end

