: base64-padding-char ( -- c) '=' ;

: base64-padding-char-num ( addr u -- u)
\ gets the number of the padding characters at the end of the specified string
  0 >r
  begin
      1- 2dup
      chars + c@
      base64-padding-char <> dup invert
      if
          r> 1+ >r
      endif
  until 
  2drop
  r> ;

: base64-emit-value ( u --)
\ maps a value between 0 and 63 to the corresponding character and emits it
  s" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"
  drop swap
  chars + c@ emit ;

: /ceil ( n1 n2 -- n )
\ devides the first integer by the second and rounds to the next integer if necessary
  /mod swap
  0> if 1+ endif ;

: base64-encode-padding-char-num ( u -- u )
\ calculates the size of the base64 encoded string by the size of the input
  dup 3 /ceil 4 * 
  swap
  4 * 3 /ceil
  - ;

: base64-decode-len ( addr u -- u )
\ calculates the size of the decoded string by the size of the encoded string
  2dup base64-padding-char-num >r
  4 / 3 * r> -
  nip ;

: next-byte ( addr u -- b b )
\ takes a char/byte from a source and puts it onto the stack two times
  + c@ dup
  ;

: take-first-six-bits ( b -- b )
\ takes the first 6 bits of a byte
  2 rshift 
  ;

: take-last-two-bits ( b -- b )
\ takes the last 2 bits of a byte and put them in this position: 00110000
  3 and 4 lshift
  ;

: take-last-four-bits ( b -- b )
\ takes the last 4 bits of a byte and put them in this position: 00111100
  15 and 2 lshift
  ;

: take-last-six-bits ( b -- b )
\ takes the last 4 bits of a byte and put them in this position: 00111111
  63 and
  ;

: merge-with-next-four-bits ( b1 b2 b2 -- b )
\ take the first half of b1 and second half of b2 to form another byte
  4 rshift rot or
  ;

: merge-with-next-two-bits ( b1 b2 b2 -- b2 b )
\ take the first 6 bits of b1 and second half of b2 to form another byte
  6 rshift rot or
  ;

: >base64 { src src-len -- }
  src-len 0 u+do
    \ take next byte from source
    src i next-byte

    i 3 mod case
    0 of 
      \ map first six bits and emit
      take-first-six-bits base64-emit-value
      take-last-two-bits
    endof
    1 of
      \ combine four bits with previous two bits and emit
      merge-with-next-four-bits base64-emit-value
      take-last-four-bits
    endof
    2 of
      \ combine two bits with previous four bits
      merge-with-next-two-bits base64-emit-value
      \ map last six bits and emit
      take-last-six-bits base64-emit-value
    endof
    endcase

  loop
  \ emit the remaining bits if necessary
  src-len 3 mod 0> if base64-emit-value endif
  \ emit the padding characters
  src-len base64-encode-padding-char-num 0 u+do base64-padding-char emit loop
  ;

: base64>c ( u -- u )
\ maps a Base64 character to its position in the B64 encoding (between 0 and 63) by a map
  s" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/" drop
  63 0 u+do
    2dup i chars + c@ 
    = if i rot rot endif
  loop
  2drop
  ;

: base64>c-alt  
\ calculates a Base64 character's position in the B64 encoding (between 0 and 63)
  dup 96 > if 
    71 -
  else
    dup 64 > if
      65 -
    else
      dup 47 > if
        4 +
      else \ + and /
        dup 47 = if
          drop 63
        else
          drop 62
        endif
      endif
    endif
  endif
  ;

: emit-ascii-as-char
\ take a character as a number and emit it
  pad c! pad 1 type
  ;

: take-all-encoded
\ take all encoded (6) bits and shift them to the left end: (11111100)
  2 lshift 
  ;

: merge-with-next-two-encoded
\ take the bits (00110000) and add them in this position (00000011)
  dup 48 and 4 rshift rot or
  ;

: take-last-four-encoded
\ take the bits (00001111) and move them to this position (11110000)
  15 and 4 lshift
  ;

: merge-with-next-four-encoded
\ take the bits (00111100) and add them in this position (00001111)
  dup 60 and 2 rshift rot or 
  ;

: take-last-two-encoded
\ take the bits (00000011) and move them to this position (11000000)
  3 and 6 lshift
  ;


: base64> { src src-len --  }
  src-len 0 u+do
    \ take the next byte from the source and transform it into a base64 position
    src i + c@ dup
    base64-padding-char <> if
    base64>c

    i 4 mod case
    0 of 
      \ move last six bits to the left end
      take-all-encoded
    endof
    1 of
      \ merge previous six bits with the next encoded four and emit
      merge-with-next-two-encoded
      emit-ascii-as-char
      \ move the remaining four encoded bits to the left end
      take-last-four-encoded
    endof
    2 of
      \ merge previous four bits with the next encoded four bits and emit
      merge-with-next-four-encoded
      emit-ascii-as-char
      \ move the remaining two encoded bits to the left end
      take-last-two-encoded
    endof
    3 of
      \ merge previous two bits with the next encoded six bits and emit
      or emit-ascii-as-char
    endof
    endcase
    else 
     \ ignore padding chars
     drop
    endif
  loop
  ;


