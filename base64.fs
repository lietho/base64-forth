: base64-padding-char ( -- c) '=' ;

: normalize-char ( c -- c) 256 mod ;

: base64-padding-char-num ( addr u -- u)
\ gets the number of the padding characters at the end of the specified string
    0 >r
    begin
        1- 2dup
        chars + @ normalize-char
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
    chars + @ ;

: base64-encode-len ( addr u -- u )
\ calculates the size of the base64 encoded string by the size of the input
    nip 3 /mod swap 0>
    if 1+ endif
    4 * ;

: base64-decode-len ( addr u -- u )
\ calculates the size of the decoded string by the size of the encoded string ((3 * (LengthInCharacters / 4)) - (numberOfPaddingCharacters))
   2dup base64-padding-char-num >r
   4 / 3 * r> - ;

: base64-encode ( addr u -- addr u )
\ IDEA: Allocate memory for result, iterate over input, map values, set corresponding value in result
    here swap base64-encode-len dup chars allot \ reserve memory for the encoded string
    rot drop \ just cleaning up as long we aren't finished
    ;

: base64-decode ( addr u -- addr u )
\ IDEA: Allocate memory for result, iterate over input, map values, set corresponding value in result
    here swap base64-decode-len dup chars allot \ reserve memory for the encoded string
    rot drop \ just cleaning up as long we aren't finished
    ;