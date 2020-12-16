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

    i 3 mod 0 = if
      \ map first six bits and emit
      take-first-six-bits base64-emit-value
      take-last-two-bits
    endif

    i 3 mod 1 = if
      \ combine four bits with previous two bits and emit
      merge-with-next-four-bits base64-emit-value
      take-last-four-bits
    endif

    i 3 mod 2 = if
      \ combine two bits with previous four bits
      merge-with-next-two-bits base64-emit-value
      \ map last six bits and emit
      take-last-six-bits base64-emit-value
    endif

  loop
  \ emit the remaining bits if necessary
  src-len 3 mod 0> if base64-emit-value endif
  \ emit the padding characters
  src-len base64-encode-padding-char-num 0 u+do base64-padding-char emit loop
  ;


: base64> { src src-len --  }
  ;
