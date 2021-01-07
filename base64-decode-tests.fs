s" testsuite.fs" included
s" base64.fs" included

: string-base64> ( addr u -- addr u )
  ['] base64> >string-execute ;

s" "          string-base64> s" "        test
s" Zg=="      string-base64> s" f"       test
s" Zm8="      string-base64> s" fo"      test
s" Zm9v"      string-base64> s" foo"     test
s" Zm9vYg=="  string-base64> s" foob"    test
s" Zm9vYmE="  string-base64> s" fooba"   test
s" Zm9vYmFy"  string-base64> s" foobar"  test                                               