s" testsuite.fs" included
s" base64.fs" included

s" Zm9vYg=="  base64-padding-char-num 2 <> throw
s" Zm9vYmE="  base64-padding-char-num 1 <> throw
s" Zm9vYmFy"  base64-padding-char-num 0 <> throw

s" "          base64-decode-len 0 <> throw
s" Zg=="      base64-decode-len 1 <> throw
s" Zm8="      base64-decode-len 2 <> throw
s" Zm9v"      base64-decode-len 3 <> throw
s" Zm9vYg=="  base64-decode-len 4 <> throw
s" Zm9vYmE="  base64-decode-len 5 <> throw
s" Zm9vYmFy"  base64-decode-len 6 <> throw

s" "          base64> s" "        test
s" Zg=="      base64> s" f"       test
s" Zm8="      base64> s" fo"      test
s" Zm9v"      base64> s" foo"     test
s" Zm9vYg=="  base64> s" foob"    test
s" Zm9vYmE="  base64> s" fooba"   test
s" Zm9vYmFy"  base64> s" foobar"  test