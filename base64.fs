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

: base64-encode { src src-len -- addr u }
    src src-len
    base64-encode-len here swap
    dup chars allot
    fill-with-base64-padding-char
    0 0
    { dst dst-len src-idx dst-idx }
    dst-len 0<> if
        begin
            src src-idx + c@ dup
            1 src-idx + to src-idx
            2 rshift
            base64-map-value dst dst-idx + c!
            1 dst-idx + to dst-idx

            3 and 4 lshift
            src-idx src-len < if
                src src-idx + c@ dup
                1 src-idx + to src-idx
                4 rshift rot or

                base64-map-value dst dst-idx + c!
                1 dst-idx + to dst-idx

                15 and 2 lshift
                src-idx src-len < if
                    src src-idx + c@ dup
                    1 src-idx + to src-idx
                    6 rshift rot or

                    base64-map-value dst dst-idx + c!
                    1 dst-idx + to dst-idx
                    63 and
                    base64-map-value dst dst-idx + c!
                    1 dst-idx + to dst-idx
                else
                    base64-map-value dst dst-idx + c!
                    1 dst-idx + to dst-idx
                endif
            else
                base64-map-value dst dst-idx + c!
                1 dst-idx + to dst-idx
            endif

        src-idx src-len >= until
    endif
    dst dst-len
    ;

: base64-decode { src src-len -- addr u }
\ IDEA: Allocate memory for result, iterate over input, map values, set corresponding value in result
    src src-len
    base64-encode-len here swap
    dup chars allot
    ;