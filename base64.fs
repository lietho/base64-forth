: map-value ( u -- c)
\ maps a value between 0 and 63 to the corresponding character
    s" ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"
    drop swap
    chars + @ ;

: base64-encode ( addr u -- addr u )
    noop ;

: base64-decode ( addr u -- addr u )
    noop ;