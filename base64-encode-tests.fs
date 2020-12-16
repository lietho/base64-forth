s" testsuite.fs" included
s" base64.fs" included

0 base64-encode-padding-char-num 0 <> throw
1 base64-encode-padding-char-num 2 <> throw
2 base64-encode-padding-char-num 1 <> throw
3 base64-encode-padding-char-num 0 <> throw
4 base64-encode-padding-char-num 2 <> throw
5 base64-encode-padding-char-num 1 <> throw
6 base64-encode-padding-char-num 0 <> throw

\ taken from https://tools.ietf.org/html/rfc4648#section-10
\ TODO: re-implement with >string-execute
\ s" "        >base64 s" "          test
\ s" f"       >base64 s" Zg=="      test
\ s" fo"      >base64 s" Zm8="      test
\ s" foo"     >base64 s" Zm9v"      test
\ s" foob"    >base64 s" Zm9vYg=="  test
\ s" fooba"   >base64 s" Zm9vYmE="  test
\ s" foobar"  >base64 s" Zm9vYmFy"  test
