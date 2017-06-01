<?xml version="1.0"?>

<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output omit-xml-declaration="yes" method="html" indent="no" version="5.0" />
  <xsl:template match="test-run">
    <html lang="en">
      <head>
        <title>Testsuite Results</title>
        <meta charset="utf-8"></meta>
        <meta name="viewport" content="width=device-width, initial-scale=1"></meta>
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">//</link>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js">//</script>
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js">//</script>
      </head>
      <body>
        <div class="container">
          <h2>
            Test Run Results
          </h2>
          <h3>
            Test Suite
          </h3>
          <div class="table-responsive">
            <table class="table table-striped table-bordered table-list">
              <thead>
                <tr>
                  <th>Result</th>
                  <th>Duration</th>
                  <th>Start</th>
                  <th>End</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>
                    <xsl:if test="@result='Passed'">
                      <button type="button" class="btn btn-success">
                        <xsl:value-of select="@result" />
                      </button>
                    </xsl:if>
                    <xsl:if test="@result='Failed'">
                      <button type="button" class="btn btn-danger">
                        <xsl:value-of select="@result" />
                      </button>
                    </xsl:if>
                  </td>
                  <td>
                    <xsl:value-of select="@duration" />
                  </td>
                  <td>
                    <xsl:value-of select="@start-time" />
                  </td>
                  <td>
                    <xsl:value-of select="@end-time" />
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <h3>
            Tests
          </h3>
          <div class="table-responsive">
            <table class="table table-striped table-bordered table-list">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Description</th>
                  <th>Result</th>
                  <th>Duration</th>
                  <th>Message</th>
                </tr>
              </thead>
              <tbody>
                <xsl:for-each select="test-suite/test-suite/test-suite/test-suite/test-suite/test-suite/test-case">
                  <tr>
                    <td>
                      <xsl:value-of select="@name" />
                    </td>
                    <td>
                      <xsl:for-each select="properties/property">
                        <xsl:if test="@name='_DESCRIPTION'">
                          <xsl:value-of select="@value" />
                        </xsl:if>
                      </xsl:for-each>
                    </td>
                    <td>
                      <xsl:if test="@result='Passed'">
                        <button type="button" class="btn btn-success">
                          <xsl:value-of select="@result" />
                        </button>
                      </xsl:if>
                      <xsl:if test="@result='Failed'">
                        <button type="button" class="btn btn-danger">
                          <xsl:value-of select="@result" />
                        </button>
                      </xsl:if>
                    </td>
                    <td>
                      <xsl:value-of select="@duration" />
                    </td>
                    <xsl:if test="failure/message">
                      <td>
                        <div style="white-space: pre;">
                          <xsl:value-of select="failure/message" />
                        </div>
                      </td>
                    </xsl:if>
                    <xsl:if test="not(failure/message)">
                      <td>
                      </td>
                    </xsl:if>
                  </tr>
                </xsl:for-each>
              </tbody>
            </table>
          </div>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>