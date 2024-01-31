.data

    corner  dd 1.0, 1.0, 1.0, 1.0
    diag    dd 2.0, 2.0, 2.0, 2.0
    center  dd 4.0, 4.0, 4.0, 4.0
    divide  dd 16.0, 16.0, 16.0, 16.0
    minus   dd -1.0, -1.0, -1.0, -1.0
    laplace dd 8.0, 8.0, 8.0, 8.0

    redMask   dd 00FF0000h
    greenMask dd 0000FF00h
    blueMask  dd 000000FFh

.code

GaussianBlur proc
    ; RCX - pointer to first element of the array
    ; RDX - start idx (9, czyli drugi piksel drugiego wiersza)
    ; R8  - end idx (54, ostatni indeks do przetworzenia)
    ; R9  - empty array to fill
    ; RSI - row counter

    mov RSI, 15         ; Initialize row counter (end of the second row)

    mov RBX, RDX         ; Copy start index to R8
    add RBX, RBX
    add RBX, RBX          ; Multiply start index (R8) by 4 (to get byte offset)
    add RCX, RBX         ; Move to section start
    add R9, RBX         ; Move to section start

startLoop: 

    xorps xmm0, xmm0                ; Inicjalizacja xmm0, ktory bedzie przechowywal nowe wartosci piksela              

    mov eax, dword ptr [RCX - 36]    ; Lewy G�rny s�siad [X - -]
    ;movups xmm9, dword ptr [corner] ; Za�aduj wag� 1.0  [- X -]
    call processPixel                ;                   [- - -]

    mov eax, dword ptr [RCX - 32]    ; G�rny s�siad      [- X -]
    movups xmm9, dword ptr [diag]    ; Za�aduj wag� 2.0  [- - -]
    call processPixel                ;                   [- - -]

    mov eax, dword ptr [RCX - 28]    ; Prawy g�rny s�siad[- - X]
    movups xmm9, dword ptr [corner]  ; Za�aduj wag� 1.0  [- - -]
    call processPixel                ;                   [- - -]

    mov eax, dword ptr [RCX - 4]     ; Lewy s�siad       [- - -]
    movups xmm9, dword ptr [diag]    ; Za�aduj wag� 2.0  [X - -]
    call processPixel                ;                   [- - -]

    mov eax, dword ptr [RCX]         ; Bie��cy piksel    [- - -]
    movups xmm9, dword ptr [center]  ; Za�aduj wag� 4.0  [- X -]
    call processPixel                ;                   [- - -]

    mov eax, dword ptr [RCX + 4]     ; Prawy s�siad      [- - -]
    movups xmm9, dword ptr [diag]    ; Za�aduj wag� 2.0  [- - X]
    call processPixel                ;                   [- - -]

    mov eax, dword ptr [RCX + 28]    ; Lewy dolny s�siad [- - -]
    movups xmm9, dword ptr [corner]  ; Za�aduj wag� 1.0  [- - -]
    call processPixel                ;                   [X - -]

    mov eax, dword ptr [RCX + 32]    ; Dolny s�siad      [- - -]
    movups xmm9, dword ptr [diag]    ; Za�aduj wag� 2.0  [- - -]
    call processPixel                ;                   [- X -]

    mov eax, dword ptr [RCX + 36]    ; Prawy dolny s�siad[- - -]
    movups xmm9, dword ptr [corner]  ; Za�aduj wag� 1.0  [- - -]
    call processPixel                ;                   [- - X]

    divps xmm0, divide 

    cvtps2dq xmm0, xmm0             ; Konwertuj zmiennoprzecinkowe warto�ci na ca�kowite
    packusdw xmm0, xmm0             ; Pakuj warto�ci do 16-bitowych warto�ci
    packuswb xmm0, xmm0             ; Pakuj warto�ci do 8-bitowych warto�ci
    movd eax, xmm0                  ; Przenie� warto�� do rejestru og�lnego przeznaczenia
    and eax, 00FFFFFFh              ; Ustaw kana� Alpha na 0x00
 
    mov [RCX], eax
    mov [R9], eax                  ; Zapisz wynik do pami�ci

    add RCX, 4				;Increment array pinter
    add R9, 4				;Increment array pinter
    inc RDX					;Increment array index

    cmp RDX, RSI        ; Check if the end of the row
    jl continueLoop     ; If not end of the row, continue

    ; Skip last element of the row and the first element of the next row
    add RCX, 8
    add R9, 8
    add RDX, 2

    ; Increment row counter to the end of the next row
    add RSI, 8

continueLoop:
    cmp RDX, R8        ; Check if reached the end index
    jl startLoop        ; If not reached, continue loop

    xor rax, rax        ; Test
    add rax, 10
    ret

processPixel:
                          ; Ekstrakcja kana��w i konwersja na format zmiennoprzecinkowy
    mov R10d, eax              ; Przekazanie piksela do rejestru R9
    and R10d, redMask          ; Ekstrakcja kana�u czerwonego
    shr R10d, 16               ; Przesuniecie kanalu na LSB
    cvtsi2ss xmm1, R10d        ; Konwersja czerwonego na zmiennoprzecinkowe i za�adowanie do xmm0

    mov R11d, eax             ; Przekazanie piksela do rejestru R10
    and R11d, greenMask       ; Ekstrakcja kana�u zielonego
    shr R11d, 8               ; Przesuniecie kanalu na LSB
    cvtsi2ss xmm2, R11d       ; Konwersja zielonego na zmiennoprzecinkowe i za�adowanie do xmm1

    mov R12d, eax             ; Przekazanie piksela do rejestru R11
    and R12d, blueMask        ; Ekstrakcja kana�u niebieskiego
    cvtsi2ss xmm3, R12d       ; Konwersja niebieskiego na zmiennoprzecinkowe i za�adowanie do xmm2

                             ; ��czenie kana��w R, G, B w xmm0
    shufps xmm3, xmm3, 0     ; Duplikacja warto�ci R
    shufps xmm2, xmm2, 0     ; Duplikacja warto�ci G
    unpcklps xmm3, xmm2      ; ��czenie R i G
    movlhps xmm3, xmm1       ; Dodanie B do xmm0

    mulps xmm3, xmm9         ; Przemno�enie piksela przez wage
    addps xmm0, xmm3         ; Dodanie do xmm0 (zainicjalizowane)
    ret

GaussianBlur endp

LaplacianFilter proc
    ; RCX - pointer to first element of the array
    ; RDX - start idx (9, czyli drugi piksel drugiego wiersza)
    ; R8  - end idx (54, ostatni indeks do przetworzenia)
    ; R9  - empty array to fill
    ; RSI - row counter

    mov RSI, 15         ; Initialize row counter (end of the second row)

    mov RBX, RDX         ; Copy start index to R8
    add RBX, RBX
    add RBX, RBX          ; Multiply start index (R8) by 4 (to get byte offset)
    add RCX, RBX         ; Move to section start
    add R9, RBX         ; Move to section start

startLoop: 

    xorps xmm0, xmm0                ; Inicjalizacja xmm0, ktory bedzie przechowywal nowe wartosci piksela              

    mov eax, dword ptr [RCX - 36]    ; Lewy G�rny s�siad [X - -]
    ;movups xmm9, dword ptr [minus]  ; Za�aduj wag� 1.0  [- X -]
    call processPixel                ;                   [- - -]

    mov eax, dword ptr [RCX - 32]    ; G�rny s�siad      [- X -]
    movups xmm9, dword ptr [minus]   ; Za�aduj wag� 2.0  [- - -]
    call processPixel                ;                   [- - -]

    mov eax, dword ptr [RCX - 28]    ; Prawy g�rny s�siad[- - X]
    movups xmm9, dword ptr [minus]   ; Za�aduj wag� 1.0  [- - -]
    call processPixel                ;                   [- - -]

    mov eax, dword ptr [RCX - 4]     ; Lewy s�siad       [- - -]
    movups xmm9, dword ptr [minus]   ; Za�aduj wag� 2.0  [X - -]
    call processPixel                ;                   [- - -]

    mov eax, dword ptr [RCX]         ; Bie��cy piksel    [- - -]
    movups xmm9, dword ptr [laplace] ; Za�aduj wag� 4.0  [- X -]
    call processPixel                ;                   [- - -]

    mov eax, dword ptr [RCX + 4]     ; Prawy s�siad      [- - -]
    movups xmm9, dword ptr [minus]   ; Za�aduj wag� 2.0  [- - X]
    call processPixel                ;                   [- - -]

    mov eax, dword ptr [RCX + 28]    ; Lewy dolny s�siad [- - -]
    movups xmm9, dword ptr [minus]   ; Za�aduj wag� 1.0  [- - -]
    call processPixel                ;                   [X - -]

    mov eax, dword ptr [RCX + 32]    ; Dolny s�siad      [- - -]
    movups xmm9, dword ptr [minus]   ; Za�aduj wag� 2.0  [- - -]
    call processPixel                ;                   [- X -]

    mov eax, dword ptr [RCX + 36]    ; Prawy dolny s�siad[- - -]
    movups xmm9, dword ptr [minus]   ; Za�aduj wag� 1.0  [- - -]
    call processPixel                ;                   [- - X]

    cvtps2dq xmm0, xmm0             ; Konwertuj zmiennoprzecinkowe warto�ci na ca�kowite
    packusdw xmm0, xmm0             ; Pakuj warto�ci do 16-bitowych warto�ci
    packuswb xmm0, xmm0             ; Pakuj warto�ci do 8-bitowych warto�ci
    movd eax, xmm0                  ; Przenie� warto�� do rejestru og�lnego przeznaczenia
    and eax, 00FFFFFFh              ; Ustaw kana� Alpha na 0x00
 
    mov [RCX], eax
    mov [R9], eax                  ; Zapisz wynik do pami�ci

    add RCX, 4				;Increment array pinter
    add R9, 4				;Increment array pinter
    inc RDX					;Increment array index

    cmp RDX, RSI        ; Check if the end of the row
    jl continueLoop     ; If not end of the row, continue

    ; Skip last element of the row and the first element of the next row
    add RCX, 8
    add R9, 8
    add RDX, 2

    ; Increment row counter to the end of the next row
    add RSI, 8

continueLoop:
    cmp RDX, R8        ; Check if reached the end index
    jl startLoop        ; If not reached, continue loop

    xor rax, rax        ; Test
    add rax, 10
    ret

processPixel:
                          ; Ekstrakcja kana��w i konwersja na format zmiennoprzecinkowy
    mov R10d, eax              ; Przekazanie piksela do rejestru R9
    and R10d, redMask          ; Ekstrakcja kana�u czerwonego
    shr R10d, 16               ; Przesuniecie kanalu na LSB
    cvtsi2ss xmm1, R10d        ; Konwersja czerwonego na zmiennoprzecinkowe i za�adowanie do xmm0

    mov R11d, eax             ; Przekazanie piksela do rejestru R10
    and R11d, greenMask       ; Ekstrakcja kana�u zielonego
    shr R11d, 8               ; Przesuniecie kanalu na LSB
    cvtsi2ss xmm2, R11d       ; Konwersja zielonego na zmiennoprzecinkowe i za�adowanie do xmm1

    mov R12d, eax             ; Przekazanie piksela do rejestru R11
    and R12d, blueMask        ; Ekstrakcja kana�u niebieskiego
    cvtsi2ss xmm3, R12d       ; Konwersja niebieskiego na zmiennoprzecinkowe i za�adowanie do xmm2

                             ; ��czenie kana��w R, G, B w xmm0
    shufps xmm3, xmm3, 0     ; Duplikacja warto�ci R
    shufps xmm2, xmm2, 0     ; Duplikacja warto�ci G
    unpcklps xmm3, xmm2      ; ��czenie R i G
    movlhps xmm3, xmm1       ; Dodanie B do xmm0

    mulps xmm3, xmm9         ; Przemno�enie piksela przez wage
    addps xmm0, xmm3         ; Dodanie do xmm0 (zainicjalizowane)
    ret

LaplacianFilter endp

end

