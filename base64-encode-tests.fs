s" Zm9vYg=="  base64-padding-char-num 2 <> throw
s" Zm9vYmE="  base64-padding-char-num 1 <> throw
s" Zm9vYmFy"  base64-padding-char-num 0 <> throw

s" "        base64-encode-len 0 <> throw
s" f"       base64-encode-len 4 <> throw
s" fo"      base64-encode-len 4 <> throw
s" foo"     base64-encode-len 4 <> throw
s" foob"    base64-encode-len 8 <> throw
s" fooba"   base64-encode-len 8 <> throw
s" foobar"  base64-encode-len 8 <> throw

\ taken from https://tools.ietf.org/html/rfc4648#section-10
s" "        base64-encode s" "          test
s" f"       base64-encode s" Zg=="      test
s" fo"      base64-encode s" Zm8="      test
s" foo"     base64-encode s" Zm9v"      test
s" foob"    base64-encode s" Zm9vYg=="  test
s" fooba"   base64-encode s" Zm9vYmE="  test
s" foobar"  base64-encode s" Zm9vYmFy"  test
