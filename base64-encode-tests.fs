s" testsuite.fs" included
s" base64.fs" included

s" "        base64-encode-len 0 <> throw
s" f"       base64-encode-len 4 <> throw
s" fo"      base64-encode-len 4 <> throw
s" foo"     base64-encode-len 4 <> throw
s" foob"    base64-encode-len 8 <> throw
s" fooba"   base64-encode-len 8 <> throw
s" foobar"  base64-encode-len 8 <> throw

\ taken from https://tools.ietf.org/html/rfc4648#section-10
s" "        >base64 s" "          test
s" f"       >base64 s" Zg=="      test
s" fo"      >base64 s" Zm8="      test
s" foo"     >base64 s" Zm9v"      test
s" foob"    >base64 s" Zm9vYg=="  test
s" fooba"   >base64 s" Zm9vYmE="  test
s" foobar"  >base64 s" Zm9vYmFy"  test
