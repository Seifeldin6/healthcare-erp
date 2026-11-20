import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:flutter_svg/svg.dart';
import 'package:healthcare_app/core/helpers/constants.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class AuthBrandHeader extends StatelessWidget {
  const AuthBrandHeader({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        SvgPicture.asset(
          AppAssets.authBrandLogo,
          width: 40.w,
          height: 40.w,
        ),
        SizedBox(width: AppDimensions.spacing12.w),
        Text(
          s.appName,
          style: AppTextStyles.heading18InkSemiBold,
        ),
      ],
    );
  }
}