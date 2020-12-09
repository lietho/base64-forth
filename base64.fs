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

: base64-map-value ( u -- c)
\ maps a value between 0 and 63 to the corresponding character
    s" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"
    drop swap
    chars + c@ ;

: base64-encode-len ( addr u -- u )
\ calculates the size of the base64 encoded string by the size of the input
    nip 3 /mod swap 0>
    if 1+ endif
    4 * ;

: base64-decode-len ( addr u -- u )
\ calculates the size of the decoded string by the size of the encoded string
   2dup base64-padding-char-num >r
   4 / 3 * r> -
   nip ;

: fill-with-base64-padding-char ( addr u -- addr u) 
    2dup base64-padding-char fill ;

: next-byte ( n n -- b b )
\ takes a char/byte from a source and puts it onto the stack two times
  + c@ dup
  ;

: write-to-dst ( c n n -- )
\ writes the character in c to the location specified by the two values on top
  + c!
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
\ takes the last 4 bits of a byte and put them in this position: 00111100
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

: base64-encode { src src-len -- addr u }
    src src-len
    base64-encode-len here swap
    dup chars allot
    fill-with-base64-padding-char
    0 0
    { dst dst-len src-idx dst-idx }
    dst-len 0<> if
        begin
            src src-idx next-byte
            src-idx 1+ to src-idx
            take-first-six-bits base64-map-value
            dst dst-idx write-to-dst
            dst-idx 1+ to dst-idx 

            take-last-two-bits
            src-idx src-len < if
                src src-idx next-byte
                src-idx 1+ to src-idx
		            merge-with-next-four-bits base64-map-value
                dst dst-idx write-to-dst
                dst-idx 1+ to dst-idx

		            take-last-four-bits
                src-idx src-len < if
                    src src-idx next-byte
                    src-idx 1+ to src-idx
                    merge-with-next-two-bits base64-map-value
		                dst dst-idx write-to-dst
                    dst-idx 1+ to dst-idx

		                take-last-six-bits base64-map-value
                    dst dst-idx write-to-dst
                    dst-idx 1+ to dst-idx
                else
                    base64-map-value
                    dst dst-idx write-to-dst
                    dst-idx 1+ to dst-idx
                endif
            else
                base64-map-value
                dst dst-idx write-to-dst
                dst-idx 1+ to dst-idx
            endif

        src-idx src-len >= until
    endif
    dst dst-len
    ;

: base64-decode { src src-len -- addr u }
\ IDEA: Allocate memory for result, iterate over input, map values, set corresponding value in result
    src src-len
    base64-decode-len here swap
    dup chars allot
    ;
