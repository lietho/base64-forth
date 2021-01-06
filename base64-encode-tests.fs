s" testsuite.fs" included
s" base64.fs" included

0 base64-encode-padding-char-num 0 <> throw
1 base64-encode-padding-char-num 2 <> throw
2 base64-encode-padding-char-num 1 <> throw
3 base64-encode-padding-char-num 0 <> throw
4 base64-encode-padding-char-num 2 <> throw
5 base64-encode-padding-char-num 1 <> throw
6 base64-encode-padding-char-num 0 <> throw

: >string-base64 ( addr u -- addr u )
  ['] >base64 >string-execute ;

\ taken from https://tools.ietf.org/html/rfc4648#section-10
s" "        >string-base64  s" "          test
s" f"       >string-base64  s" Zg=="      test
s" fo"      >string-base64  s" Zm8="      test
s" foo"     >string-base64  s" Zm9v"      test
s" foob"    >string-base64  s" Zm9vYg=="  test
s" fooba"   >string-base64  s" Zm9vYmE="  test
s" foobar"  >string-base64  s" Zm9vYmFy"  test