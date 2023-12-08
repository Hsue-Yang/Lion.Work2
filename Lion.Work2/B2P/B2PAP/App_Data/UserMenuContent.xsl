<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" encoding="utf-8"/>
    <xsl:template match="/">
        <a href="javascript:void(0);" id="navicon3"></a>
        <ul>
            <xsl:for-each select="MenuContents/MenuContent">
                <li class="hasmenu">
                    <a href="javascript:void(0);" class="navtxt">
                        <span>
                            <xsl:value-of select="MenuItemHeader"/>
                        </span>
                    </a>

                    <xsl:variable name="div1Length">
                        <xsl:choose>
                            <xsl:when test="MenuItems/MenuItem[@xAxis=1]">
                                <xsl:for-each select="MenuItems/MenuItem[@xAxis=1]">
                                    <xsl:sort order="descending" select="string-length(.)" data-type="number"/>
                                    <xsl:if test="position() = 1">
                                        <xsl:copy-of select="string-length(.)" />
                                    </xsl:if>
                                </xsl:for-each>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="0" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:variable>
                    <xsl:variable name="div2Length">
                        <xsl:choose>
                            <xsl:when test="MenuItems/MenuItem[@xAxis=2]">
                                <xsl:for-each select="MenuItems/MenuItem[@xAxis=2]">
                                    <xsl:sort order="descending" select="string-length(.)" data-type="number"/>
                                    <xsl:if test="position() = 1">
                                        <xsl:copy-of select="string-length(.)" />
                                    </xsl:if>
                                </xsl:for-each>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="0" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:variable>
                    <xsl:variable name="div3Length">
                        <xsl:choose>
                            <xsl:when test="MenuItems/MenuItem[@xAxis=3]">
                                <xsl:for-each select="MenuItems/MenuItem[@xAxis=3]">
                                    <xsl:sort order="descending" select="string-length(.)" data-type="number"/>
                                    <xsl:if test="position() = 1">
                                        <xsl:copy-of select="string-length(.)" />
                                    </xsl:if>
                                </xsl:for-each>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="0" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:variable>
                    <xsl:variable name="div4Length">
                        <xsl:choose>
                            <xsl:when test="MenuItems/MenuItem[@xAxis=4]">
                                <xsl:for-each select="MenuItems/MenuItem[@xAxis=4]">
                                    <xsl:sort order="descending" select="string-length(.)" data-type="number"/>
                                    <xsl:if test="position() = 1">
                                        <xsl:copy-of select="string-length(.)" />
                                    </xsl:if>
                                </xsl:for-each>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="0" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:variable>
                    <xsl:variable name="div5Length">
                        <xsl:choose>
                            <xsl:when test="MenuItems/MenuItem[@xAxis=5]">
                                <xsl:for-each select="MenuItems/MenuItem[@xAxis=5]">
                                    <xsl:sort order="descending" select="string-length(.)" data-type="number"/>
                                    <xsl:if test="position() = 1">
                                        <xsl:copy-of select="string-length(.)" />
                                    </xsl:if>
                                </xsl:for-each>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="0" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:variable>

                    <div class="subnav" style="width:{($div1Length+$div2Length+$div3Length+$div4Length+$div5Length+1)*16}px;">
                        <ul>
                            <xsl:for-each select="MenuItems/MenuItem[@xAxis=1]">
                                <li>
                                    <a>
                                        <xsl:attribute name="href">
                                            <xsl:value-of select="@href" />
                                        </xsl:attribute>
                                        <xsl:value-of select="." />
                                    </a>
                                </li>
                            </xsl:for-each>
                        </ul>
                        <ul>
                            <xsl:for-each select="MenuItems/MenuItem[@xAxis=2]">
                                <li>
                                    <a>
                                        <xsl:attribute name="href">
                                            <xsl:value-of select="@href" />
                                        </xsl:attribute>
                                        <xsl:value-of select="." />
                                    </a>
                                </li>
                            </xsl:for-each>
                        </ul>
                        <ul>
                            <xsl:for-each select="MenuItems/MenuItem[@xAxis=3]">
                                <li>
                                    <a>
                                        <xsl:attribute name="href">
                                            <xsl:value-of select="@href" />
                                        </xsl:attribute>
                                        <xsl:value-of select="." />
                                    </a>
                                </li>
                            </xsl:for-each>
                        </ul>
                        <ul>
                            <xsl:for-each select="MenuItems/MenuItem[@xAxis=4]">
                                <li>
                                    <a>
                                        <xsl:attribute name="href">
                                            <xsl:value-of select="@href" />
                                        </xsl:attribute>
                                        <xsl:value-of select="." />
                                    </a>
                                </li>
                            </xsl:for-each>
                        </ul>
                        <ul>
                            <xsl:for-each select="MenuItems/MenuItem[@xAxis=5]">
                                <li>
                                    <a>
                                        <xsl:attribute name="href">
                                            <xsl:value-of select="@href" />
                                        </xsl:attribute>
                                        <xsl:value-of select="." />
                                    </a>
                                </li>
                            </xsl:for-each>
                        </ul>
                    </div>
                    
                </li>
            </xsl:for-each>
        </ul>
    </xsl:template>
</xsl:stylesheet>