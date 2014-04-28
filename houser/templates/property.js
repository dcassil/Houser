{% 
    _.each(alt.propData, function(prop) {
    var address = prop.Address.split(",");
    %}
    <div class="propPage" ID={{prop.AccountNumber}}>
        <span>
            <p class="address"> {{ address[0] }} </p>
            <p class="city"> {{ address[1] }} </p>
        </span>
        <span class="propdata">
            <table class="propTable">
                <tr class="row">
                    <td class="tCell">
                        <span class="title">SQFT</span>
                    </td>
                    <td class="vCell">
                        <span class="value">{{prop.Sqft }}</span>
                    </td>
                </tr>
                <tr class="row">
                    <td class="tCell">
                        <span class="title">Beds</span>
                    </td>
                    <td class="vCell">
                        <span class="value">{{prop.Beds }}</span>
                    </td>
                </tr>
                <tr class="row">
                    <td class="tCell">
                        <span class="title">Baths</span>
                    </td>
                    <td class="vCell">
                        <span class="value">{{prop.Baths }}</span>
                    </td>
                </tr>
                <tr class="row">
                    <td class="tCell">
                        <span class="title">Price</span>
                    </td>
                    <td class="vCell">
                        <span class="value">{{prop.SalePrice }}</span>
                    </td>
                </tr>
            </table>
            
        </span>
        <span class="imgWrapper"><img class="img" src={{prop.ImgPath}} /></span>
        <input type="button" value="Add to review list"/>
        <div class="notes">
            <textarea rows="4" cols="20">{{prop.Note}}</textarea>
        </div>
        
        
    </div>
    {%
    });
    %}